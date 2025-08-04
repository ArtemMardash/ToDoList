using SyncService.Core.Entities;

namespace SyncService.Features.Shared.Repositories;

public interface ITaskSyncMappingRepository
{
    /// <summary>
    /// Method to add new task sync mapping
    /// </summary>
    public Task<Guid> AddTaskSyncMappingAsync(TaskSyncMapping taskSyncMapping, CancellationToken cancellationToken);

    /// <summary>
    /// Method to delete task sync mapping
    /// </summary>
    public Task DeleteTaskSyncMappingByTaskIdAsync(Guid taskId, CancellationToken cancellationToken);

    /// <summary>
    /// Method to get task sync mapping
    /// </summary>
    public Task<TaskSyncMapping> GetTaskSyncMappingAsyncByTaskId(Guid taskId, CancellationToken cancellationToken);

    /// <summary>
    /// Method to ger task sync mapping by calendar Id
    /// </summary>
    public Task<TaskSyncMapping> GetTaskSyncMappingByCalendarIdAsync(string calendarId, CancellationToken cancellationToken);

    public Task UpdateCalendarEventIdAsync(Guid taskId, string calendarId, CancellationToken cancellationToken);
}