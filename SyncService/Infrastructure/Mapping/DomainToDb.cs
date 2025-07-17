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
            UserId = userSyncState.UserId,
            GoogleAccessToken = userSyncState.GoogleAccessToken,
            GoogleRefreshToken = userSyncState.GoogleRefreshToken,
            TokenExpiry = userSyncState.TokenExpiry
        };
    }
}