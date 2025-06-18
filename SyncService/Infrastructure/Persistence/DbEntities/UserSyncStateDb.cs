namespace SyncService.Infrastructure.Persistence.DbEntities;

public class UserSyncStateDb
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string GoogleAccessToken { get; set; }

    public string GoogleRefreshToken { get; set; }

    public DateTime TokenExpiry { get; set; }
}