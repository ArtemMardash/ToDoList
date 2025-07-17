using SharedKernel;

namespace SyncService.BackgroundJobs.Interfaces;

public interface ITaskDeletedUseCase
{
    Task ExecuteAsync(ITaskDeleted taskDeleted, CancellationToken cancellationToken);
}