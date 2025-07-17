using SyncService.Core.Entities;
using SyncService.Infrastructure.Persistence.DbEntities;

namespace SyncService.Infrastructure.Mapping;

public static class DbToDomain
{
    public static TaskSyncMapping ToDomain(this TaskSyncMappingDb taskSyncMappingDb)
    {
        return new TaskSyncMapping(taskSyncMappingDb.Id, taskSyncMappingDb.TaskId, taskSyncMappingDb.CalendarEventId);
    }

    public static UserSyncState ToDomain(this UserSyncStateDb userSyncStateDb)
    {
        return new UserSyncState(
            userSyncStateDb.Id,
            userSyncStateDb.UserId, 
            userSyncStateDb.GoogleAccessToken,
            userSyncStateDb.GoogleRefreshToken,
            userSyncStateDb.TokenExpiry);
    }
}