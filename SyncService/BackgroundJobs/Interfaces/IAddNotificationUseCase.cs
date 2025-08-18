using SyncService.BackgroundJobs.Dtos;

namespace SyncService.BackgroundJobs.Interfaces;

public interface IAddNotificationUseCase
{
    public Task ExecuteAsync(AddNotificationDto addNotificationDto, CancellationToken cancellationToken);
}