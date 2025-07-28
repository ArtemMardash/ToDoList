using Microsoft.EntityFrameworkCore;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Mapping;

namespace SyncService.Infrastructure.Persistence.Repositories;

public class TaskSyncMappingRepository: ITaskSyncMappingRepository
{
    private readonly SyncDbContext _syncDbContext;

    public TaskSyncMappingRepository(SyncDbContext syncDbContext)
    {
        _syncDbContext = syncDbContext;
    }
    
    public async Task<Guid> AddTaskSyncMappingAsync(TaskSyncMapping taskSyncMapping, CancellationToken cancellationToken)
    {
        await _syncDbContext.TasksSyncMapping.AddAsync(taskSyncMapping.ToDb(), cancellationToken);
        return taskSyncMapping.Id;
    }
    

    public async Task DeleteTaskSyncMappingAsync(Guid id, CancellationToken cancellationToken)
    {
        var taskSyncDb = await _syncDbContext.TasksSyncMapping.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (taskSyncDb == null)
        {
            throw new InvalidOperationException("there is no taskSyncMapping with such ID");
        }

        _syncDbContext.TasksSyncMapping.Remove(taskSyncDb);
    }

    public async Task<TaskSyncMapping> GetTaskSyncMappingAsync(Guid id, CancellationToken cancellationToken)
    {
        var taskSyncDb = await _syncDbContext.TasksSyncMapping.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (taskSyncDb == null)
        {
            throw new InvalidOperationException("there is no taskSyncMapping with such ID");
        }

        return taskSyncDb.ToDomain();
    }

    public async Task<TaskSyncMapping> GetTaskSyncMappingByCalendarIdAsync(string calendarId, CancellationToken cancellationToken)
    {
        var taskSyncDb = await _syncDbContext.TasksSyncMapping.FirstOrDefaultAsync(t => t.CalendarEventId == calendarId, cancellationToken);
        if (taskSyncDb == null)
        {
            throw new InvalidOperationException("there is no taskSyncMapping with such calendarEventId");
        }

        return taskSyncDb.ToDomain();
    }

    public async Task UpdateCalendarEventIdAsync(Guid taskId, string calendarId, CancellationToken cancellationToken)
    {
        var taskSyncMappingDb =
            await _syncDbContext.TasksSyncMapping.FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);
        if (taskSyncMappingDb == null)
        {
            throw new InvalidOperationException("there is no taskSyncMapping with such Id");
        }

        taskSyncMappingDb.CalendarEventId = calendarId;
    }
}