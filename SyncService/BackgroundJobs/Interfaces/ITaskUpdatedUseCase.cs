using SharedKernel;

namespace SyncService.BackgroundJobs.Interfaces;

public interface ITaskUpdatedUseCase
{
    Task ExecuteAsync(ITaskUpdated taskUpdated, CancellationToken cancellationToken);
}