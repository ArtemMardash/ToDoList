using TaskService.Core.Entities;
using TaskService.Features.Shared.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    public Task<Guid> CreateTaskAsync(Task task, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Task> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTaskAsync(Task task, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Task>> GetTasksByCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}