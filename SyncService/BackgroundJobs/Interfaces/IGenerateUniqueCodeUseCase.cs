namespace SyncService.BackgroundJobs.Interfaces;

public interface IGenerateUniqueCodeUseCase
{
    public Task ExecuteAsync(Guid userId, CancellationToken cancellationToken);
}