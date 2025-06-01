using TaskService.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Features.Shared.Repositories;

public interface ITaskRepository
{
    /// <summary>
    /// Create task
    /// </summary>
    public Task<Guid> CreateTaskAsync(Task task, CancellationToken cancellationToken);

    /// <summary>
    /// Get task by id
    /// </summary>
    public Task<Task> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Update task
    /// </summary>
    public Task UpdateTaskAsync(Task task, CancellationToken cancellationToken);
    

    /// <summary>
    /// Get all tasks by category
    /// </summary>
    public Task<List<Task>> GetTasksByCategoryAsync(Guid categoryId, CancellationToken cancellationToken);

    /// <summary>
    /// Delete Task
    /// </summary>
    public Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken);
    
}