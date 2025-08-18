using Microsoft.EntityFrameworkCore;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Mapping;

namespace SyncService.Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly SyncDbContext _syncDbContext;

    public NotificationRepository(SyncDbContext syncDbContext)
    {
        _syncDbContext = syncDbContext;
    }

    public Task CreateNotificationAsync(Notification notification, CancellationToken cancellationToken)
    {
        return _syncDbContext.Notifications.AddAsync(notification.ToDb(), cancellationToken).AsTask();
    }

    public async Task DeliverNotificationAsync(Notification notification, CancellationToken cancellationToken)
    {
        var notificationDb =
            await _syncDbContext.Notifications.FirstOrDefaultAsync(n => n.Id == notification.Id, cancellationToken);
        if (notificationDb != null)
        {
            notificationDb.IsDelivered = true;
        }
        else
        {
            throw new InvalidOperationException("This notification doesn't exist");
        }
    }

    public Task<List<Notification>> GetNotDeliveredNotificationsByUserIdAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        return _syncDbContext.Notifications
            .Where(n => n.UserId == userId && n.IsDelivered == false)
            .Select(n => n.ToDomain())
            .ToListAsync(cancellationToken);
    }

    public Task<List<Notification>> GetNotDeliveredNotificationsAsync(CancellationToken cancellationToken)
    {
        return _syncDbContext.Notifications
            .Where(n => n.IsDelivered == false)
            .Select(n => n.ToDomain())
            .ToListAsync(cancellationToken);
    }
}