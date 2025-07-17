using SharedKernel;

namespace SyncService.BackgroundJobs.Interfaces;

public interface ITaskCreatedUseCase
{
    Task ExecuteAsync(ITaskCreated taskCreated,CancellationToken cancellationToken);

}