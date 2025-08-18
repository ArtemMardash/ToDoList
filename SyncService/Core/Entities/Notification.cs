namespace SyncService.Core.Entities;

public class Notification
{
    public Guid Id { get; set; }
    
    public Guid TaskId { get; set; }
    
    public Guid UserId { get; set; }
    
    public NotificationType NotificationType { get; set; }
    
    public bool IsDelivered { get; set; }

    public Notification(Guid id, Guid taskId, Guid userId, NotificationType notificationType, bool isDelivered)
    {
        Id = id;
        TaskId = taskId;
        UserId = userId;
        NotificationType = notificationType;
        IsDelivered = isDelivered;
    }
    
    public Notification(Guid taskId, Guid userId, NotificationType notificationType, bool isDelivered)
    {
        Id = Guid.NewGuid();
        TaskId = taskId;
        UserId = userId;
        NotificationType = notificationType;
        IsDelivered = isDelivered;
    }
}

public enum NotificationType
{
    Unknown=0,
    Added=1,
    Updated=2,
    Deleted=3,
}