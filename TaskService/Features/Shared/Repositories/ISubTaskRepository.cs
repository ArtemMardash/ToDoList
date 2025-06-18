using TaskService.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Features.Shared.Repositories;

public interface ISubTaskRepository
{
    /// <summary>
    /// Create subtasks
    /// </summary>
    public Task<Guid> CreateSubTaskAsync(Subtask subtask, CancellationToken cancellationToken);

    /// <summary>
    /// Uodate subtask
    /// </summary>
    public Task UpdateSubTaskAsync(Subtask subtask, CancellationToken cancellationToken);

    /// <summary>
    /// Delete subTask
    /// </summary>
    public Task DeleteSubTaskAsync(Guid id, CancellationToken cancellationToken);

    public Task<Subtask> GetSubTaskByIdAsync(Guid id, CancellationToken cancellationToken);
}