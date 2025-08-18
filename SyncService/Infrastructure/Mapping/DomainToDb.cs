using SyncService.Core.Entities;
using SyncService.Infrastructure.Persistence.DbEntities;

namespace SyncService.Infrastructure.Mapping;

public static class DomainToDb
{
    public static TaskSyncMappingDb ToDb(this TaskSyncMapping taskSyncMapping)
    {
        return new TaskSyncMappingDb
        {
            Id = taskSyncMapping.Id,
            TaskId = taskSyncMapping.TaskId,
            CalendarEventId = taskSyncMapping.CalendarEventId
        };
    }

    public static UserSyncStateDb ToDb(this UserSyncState userSyncState)
    {
        return new UserSyncStateDb
        {
            Id = userSyncState.Id,
            GoogleId = userSyncState.GoogleId,
            UserId = userSyncState.UserId,
            GoogleAccessToken = userSyncState.GoogleAccessToken,
            GoogleRefreshToken = userSyncState.GoogleRefreshToken,
            TokenExpiry = userSyncState.TokenExpiry
        };
    }

    public static TgLinksDb ToDb(this TgLinks tgLinks)
    {
        return new TgLinksDb
        {
            Id = tgLinks.Id,
            UserId = tgLinks.UserId,
            TgUserId = tgLinks.TgUserId,
            UniqueCode = tgLinks.UniqueCode
        };
    }

    public static NotificationDb ToDb(this Notification notification)
    {
        return new NotificationDb
        {
            Id = notification.Id,
            TaskId = notification.TaskId,
            UserId = notification.UserId,
            NotificationType = (int)notification.NotificationType,
            IsDelivered = notification.IsDelivered
        };
    }
}