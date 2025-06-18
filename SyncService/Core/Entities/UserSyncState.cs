namespace SyncService.Core.Entities;

public class UserSyncState
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string GoogleAccessToken { get; set; }

    public string GoogleRefreshToken { get; set; }

    public DateTime TokenExpiry { get; set; }

    public UserSyncState(Guid id, Guid userId, string googleAccessToken, string googleRefreshToken,
        DateTime tokenExpiry)
    {
        Id = id;
        UserId = userId;
        GoogleAccessToken = string.IsNullOrWhiteSpace(googleAccessToken)
            ? throw new ArgumentException("token can not be empty", nameof(googleAccessToken))
            : googleAccessToken;
        GoogleRefreshToken = string.IsNullOrWhiteSpace(googleRefreshToken)
            ? throw new ArgumentException("token can not be empty", nameof(googleRefreshToken))
            : googleRefreshToken;
        TokenExpiry = tokenExpiry > DateTime.UtcNow
            ? tokenExpiry
            : throw new ArgumentException("token expiry should be extended", nameof(tokenExpiry));
    }

    public UserSyncState(Guid userId, string googleAccessToken, string googleRefreshToken, DateTime tokenExpiry)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        GoogleAccessToken = string.IsNullOrWhiteSpace(googleAccessToken)
            ? throw new ArgumentException("token can not be empty", nameof(googleAccessToken))
            : googleAccessToken;
        GoogleRefreshToken = string.IsNullOrWhiteSpace(googleRefreshToken)
            ? throw new ArgumentException("token can not be empty", nameof(googleRefreshToken))
            : googleRefreshToken;
        TokenExpiry = tokenExpiry > DateTime.UtcNow
            ? tokenExpiry
            : throw new ArgumentException("token expiry should be extended", nameof(tokenExpiry));
    }
}