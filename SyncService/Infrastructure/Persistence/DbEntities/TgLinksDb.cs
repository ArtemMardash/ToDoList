namespace SyncService.Infrastructure.Persistence.DbEntities;

public class TgLinksDb
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public long? TgUserId { get; set; }
    
    public int? UniqueCode { get; set; }
}