namespace SyncService.Core.Entities;

public class UserSyncState
{
    /// <summary>
    /// Id of UserSync State
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id og user
    /// </summary>
    public Guid UserId { get; set; }
    
    public string GoogleId { get; set; }

    /// <summary>
    /// Access token from Google
    /// </summary>
    public string GoogleAccessToken { get; set; }

    /// <summary>
    /// Refresh token from google
    /// </summary>
    public string? GoogleRefreshToken { get; set; }

    /// <summary>
    /// Expiry date of token
    /// </summary>
    public DateTime TokenExpiry { get; set; }

    public UserSyncState(Guid id, Guid userId, string googleAccessToken, string? googleRefreshToken, string googleId ,DateTime tokenExpiry)
    {
        Id = id;
        UserId = userId;
        GoogleAccessToken = string.IsNullOrWhiteSpace(googleAccessToken)
            ? throw new ArgumentException("token can not be empty", nameof(googleAccessToken))
            : googleAccessToken;
        GoogleRefreshToken = googleRefreshToken;
        TokenExpiry = tokenExpiry > DateTime.UtcNow
            ? tokenExpiry
            : throw new ArgumentException("token expiry should be extended", nameof(tokenExpiry));
        GoogleId = googleId;
    }

    public UserSyncState(Guid userId, string googleAccessToken, string? googleRefreshToken,string googleId ,DateTime tokenExpiry)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        GoogleAccessToken = string.IsNullOrWhiteSpace(googleAccessToken)
            ? throw new ArgumentException("token can not be empty", nameof(googleAccessToken))
            : googleAccessToken;
        GoogleRefreshToken = googleRefreshToken;
        TokenExpiry = tokenExpiry > DateTime.UtcNow
            ? tokenExpiry
            : throw new ArgumentException("token expiry should be extended", nameof(tokenExpiry));
        GoogleId = googleId;
    }
}