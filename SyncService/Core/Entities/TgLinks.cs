namespace SyncService.Core.Entities;

public class TgLinks
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public long? TgUserId { get; set; }
    
    public int? UniqueCode { get; set; }

    public TgLinks(Guid id, Guid userId, long? tgUserId, int? uniqueCode)
    {
        Id = id;
        UserId = userId;
        TgUserId = tgUserId;
        UniqueCode = uniqueCode;
    }
    
    public TgLinks(Guid userId, long? tgUserId, int? uniqueCode)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TgUserId = tgUserId;
        UniqueCode = uniqueCode;
    }
}