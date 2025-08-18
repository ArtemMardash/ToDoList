using SyncService.Core.Entities;

namespace SyncService.BackgroundJobs.Interfaces;

public interface IGetUndeliveredNotificationsUseCase
{
    public Task<List<Notification>> ExecuteAsync(Guid userId, CancellationToken cancellationToken);
}