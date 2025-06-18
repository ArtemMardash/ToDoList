namespace SyncService.Infrastructure.Persistence.DbEntities;

public class TaskSyncMappingDb
{
    public Guid Id { get; set; }
    
    public Guid TaskId { get; set; }
    
    public string CalendarEventId { get; set; }
}