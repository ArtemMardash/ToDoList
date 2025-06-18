using Microsoft.EntityFrameworkCore;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure.Mapping;
using TaskService.Infrastructure.Persistence.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _dbContext;

    public TaskRepository(TaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateTaskAsync(ToDoTask task, CancellationToken cancellationToken)
    {
        await _dbContext.Tasks.AddAsync(task.ToDb(), cancellationToken);
        return task.Id;
    }

    public async Task<ToDoTask> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks
            .Include(t => t.SubTasks)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (task == null)
        {
            throw new InvalidOperationException("There is no task with such id");
        }

        return task.ToDomain();
    }

    public async Task UpdateTaskAsync(ToDoTask task, CancellationToken cancellationToken)
    {
        var taskDb = await _dbContext.Tasks
            .Include(t => t.SubTasks)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == task.Id, cancellationToken);

        if (taskDb == null)
        {
            throw new InvalidOperationException("There is no task with such id");
        }

        var subTasksForDelete = taskDb.SubTasks
            .Where(st => task.SubTasks.All(tst => tst.Id != st.Id))
            .ToList();
        taskDb.SubTasks = taskDb.SubTasks.Except(subTasksForDelete).ToList();
        foreach (var subtaskDbToUpdate in taskDb.SubTasks)
        {
            var updatedSt = task.SubTasks.FirstOrDefault(st => st.Id == subtaskDbToUpdate.Id);
            if (updatedSt == null)
            {
                continue;
            }

            subtaskDbToUpdate.TaskStatus = (int)updatedSt.TaskStatus;
            subtaskDbToUpdate.Name = updatedSt.Name;
        }

        var subTasksToAdd = task.SubTasks
            .Where(tst => taskDb.SubTasks.All(st => tst.Id != st.Id))
            .ToList();

        taskDb.SubTasks.AddRange(subTasksToAdd.Select(s => s.ToDb()));

        var categoryDb = await _dbContext.Categories.FirstOrDefaultAsync(
            c => c.Id == task.Category.Id || c.Name.ToLower() == task.Category.Name.ToLower(),
            cancellationToken);


        taskDb.TaskStatus = (int)task.TaskStatus;
        taskDb.Description = task.Description;
        taskDb.Deadline = task.Deadline;
        taskDb.Category = categoryDb ?? new CategoryDb
        {
            Id = Guid.NewGuid(),
            Name = task.Category.Name,
            Description = task.Category.Description
        };
        taskDb.Name = task.Name;
    }

    public Task<List<ToDoTask>> GetTasksByCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return _dbContext.Tasks
            .Include(t => t.Category)
            .Include(t => t.SubTasks)
            .Where(t => t.Category.Id == categoryId)
            .Select(tDb => tDb.ToDomain())
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        var taskDb = await _dbContext.Tasks.FindAsync(id, cancellationToken);
        if (taskDb == null)
        {
            throw new InvalidOperationException("There is no task with such id");
        }

        _dbContext.Tasks.Remove(taskDb);
    }
}