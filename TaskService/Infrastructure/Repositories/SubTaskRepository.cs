using TaskService.Core.Entities;
using TaskService.Features.Shared.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Infrastructure.Repositories;

public class SubTaskRepository : ISubTaskRepository
{
    public Task<Guid> CreateSubTaskAsync(SubTask subTask, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSubTaskAsync(SubTask subTask, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSubTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}