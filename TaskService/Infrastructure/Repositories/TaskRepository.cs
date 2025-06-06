using Microsoft.EntityFrameworkCore;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features.Shared.Repositories;
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
        var taskDb= await _dbContext.Tasks.AddAsync(TaskToTaskDb(task), cancellationToken);
        return taskDb.Entity.Id;
    }

    public async Task<ToDoTask> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks
            .Include(t => t.SubTasks)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (task == null)
        {
            throw new InvalidOperationException("There is no task with such id");
        }
        return TaskDbToTask(task);
    }

    public Task UpdateTaskAsync(ToDoTask task, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<ToDoTask>> GetTasksByCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private TaskDb TaskToTaskDb(ToDoTask task)
    {
        return new TaskDb
        {
            Id = task.Id,
            UserId = task.UserId,
            Name = task.Name,
            Description = task.Description,
            Category = new CategoryDb
            {
                Id = task.Category.Id,
                Name = task.Category.Name,
                Description = task.Category.Description
            },
            Deadline = task.Deadline,
            SubTasks = task.SubTasks.Select(SubTaskToSubTaskDb).ToList()
        };
    }

    private ToDoTask TaskDbToTask(TaskDb taskDb)
    {
        var taskStatus = (TaskAndSubTaskStatus)taskDb.TaskStatus;
        var task = new ToDoTask(
            taskDb.Id,
            taskDb.Name,
            taskDb.Description,
            new Category(taskDb.Category.Id, taskDb.Category.Name, taskDb.Category.Description),
            taskDb.Deadline,
            taskStatus,
            new List<SubTask>());
        
        var subTasks = taskDb.SubTasks.Select(s => SubTaskDbToSubTask(s, task));
        task.SubTasks = subTasks.ToList();
        
        return task;
    }

    private SubTaskDb SubTaskToSubTaskDb(SubTask subTask)
    {
        return new SubTaskDb
        {
            Id = subTask.Id,
            Name = subTask.Name,
            ParentId = subTask.Parent.Id,
            TaskStatus = (int) subTask.TaskStatus
        };
    }

    private SubTask SubTaskDbToSubTask(SubTaskDb subTaskDb, ToDoTask task)
    {
        return new SubTask(
            subTaskDb.Id,
            subTaskDb.Name,
            (TaskAndSubTaskStatus)subTaskDb.TaskStatus,
            task);
    }
}