using SyncService.Core.Entities;

namespace SyncService.BackgroundJobs.Dtos;

public class AddNotificationDto
{
    public Guid TaskId { get; set; }
    
    public Guid UserId { get; set; }
    
    public NotificationType NotificationType { get; set; }
    
    public bool IsDelivered { get; set; }
}