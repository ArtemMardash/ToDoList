using Microsoft.EntityFrameworkCore;
using TaskService.Core.Entities;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure.Mapping;
using TaskService.Infrastructure.Persistence.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Infrastructure.Repositories;

public class SubTaskRepository : ISubTaskRepository
{
    private readonly TaskDbContext _dbContext;

    public SubTaskRepository(TaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateSubTaskAsync(Subtask subtask, CancellationToken cancellationToken)
    {
        await _dbContext.Subtasks.AddAsync(subtask.ToDb(), cancellationToken);
        return subtask.Id;
    }

    public async Task UpdateSubTaskAsync(Subtask subtask, CancellationToken cancellationToken)
    {
        var subTaskDb = await _dbContext.Subtasks.FindAsync(subtask.Id, cancellationToken);

        if (subTaskDb == null)
        {
            throw new InvalidOperationException("There is no subtask with such Id ");
        }

        subTaskDb.TaskStatus = (int)subtask.TaskStatus;
        subTaskDb.Name = subtask.Name;
    }

    public async Task DeleteSubTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        var subTaskToDelete = await _dbContext.Subtasks.FindAsync(id, cancellationToken);

        if (subTaskToDelete == null)
        {
            throw new InvalidOperationException("There is no subTask with such Id ");
        }

        _dbContext.Subtasks.Remove(subTaskToDelete);
    }

    public async Task<Subtask> GetSubTaskByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var subTaskDb = await _dbContext.Subtasks.FindAsync(id, cancellationToken);

        if (subTaskDb == null)
        {
            throw new InvalidOperationException("There is no subTask with such Id ");
        }

        var task = await _dbContext.Tasks
            .FirstOrDefaultAsync(t => t.Id == subTaskDb.ParentId, cancellationToken);

        if (task == null)
        {
            throw new InvalidOperationException("There is no parent with such id");
        }

        return subTaskDb.ToDomain(task.ToDomain());
    }
}