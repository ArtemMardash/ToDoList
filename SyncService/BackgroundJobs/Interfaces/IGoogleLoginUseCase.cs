using SharedKernel;

namespace SyncService.BackgroundJobs.Interfaces;

public interface IGoogleLoginUseCase
{
    Task ExecuteAsync(IGoogleLogin googleLogin,CancellationToken cancellationToken);
}