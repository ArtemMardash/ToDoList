namespace SyncService.Core.Entities;

public class TaskSyncMapping
{
    /// <summary>
    /// If of TaskSyncMapping
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Id of task
    /// </summary>
    public Guid TaskId { get; set; }
    
    /// <summary>
    /// Google calendare Id
    /// </summary>
    public string CalendarEventId { get; set; }

    public TaskSyncMapping(Guid id, Guid taskId, string calendarEventId)
    {
        Id = id;
        TaskId = taskId;
        CalendarEventId = calendarEventId;
    }
    
    
    public TaskSyncMapping(Guid taskId, string calendarEventId)
    {
        Id = Guid.NewGuid();
        TaskId = taskId;
        CalendarEventId = calendarEventId;
    }
}