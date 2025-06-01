using TaskService.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Features.Shared.Repositories;

public interface ISubTaskRepository
{
    /// <summary>
    /// Create subtasks
    /// </summary>
    public Task<Guid> CreateSubTaskAsync(SubTask subTask, CancellationToken cancellationToken);

    /// <summary>
    /// Uodate subTask
    /// </summary>
    public Task UpdateSubTaskAsync(SubTask subTask, CancellationToken cancellationToken);
    
    /// <summary>
    /// Delete subTask
    /// </summary>
    public Task DeleteSubTaskAsync(Guid id, CancellationToken cancellationToken);
}