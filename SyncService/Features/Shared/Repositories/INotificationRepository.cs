using SyncService.Core.Entities;

namespace SyncService.Features.Shared.Repositories;

public interface INotificationRepository
{
    public Task CreateNotificationAsync(Notification notification, CancellationToken cancellationToken);

    public Task DeliverNotificationAsync(Notification notification, CancellationToken cancellationToken);

    public Task<List<Notification>> GetNotDeliveredNotificationsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    
    public Task<List<Notification>> GetNotDeliveredNotificationsAsync(CancellationToken cancellationToken);

}