namespace SyncService.Core.Entities;

public class TaskSyncMapping
{
    public Guid Id { get; set; }
    
    public Guid TaskId { get; set; }
    
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