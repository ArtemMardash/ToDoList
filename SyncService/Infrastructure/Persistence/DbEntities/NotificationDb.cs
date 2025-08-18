namespace SyncService.Infrastructure.Persistence.DbEntities;

public class NotificationDb
{
    public Guid Id { get; set; }
    
    public Guid TaskId { get; set; }
    
    public Guid UserId { get; set; }
    
    public int NotificationType { get; set; }
    
    public bool IsDelivered { get; set; }
}
