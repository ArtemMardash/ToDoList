using SharedKernel;

namespace SyncService.BackgroundJobs.Interfaces;

public interface IGoogleRegisterUseCase
{
    Task ExecuteAsync(IGoogleRegistered googleRegistered,CancellationToken cancellationToken);

}