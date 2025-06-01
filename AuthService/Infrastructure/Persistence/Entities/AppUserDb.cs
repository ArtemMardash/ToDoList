using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Persistence.Entities;

public class AppUserDb : IdentityUser
{
    /// <summary>
    /// Full name of user
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Refresh token of user
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Expiry date of refresh token
    /// </summary>
    public DateTime RefreshTokenExpiry { get; set; }

    /// <summary>
    /// Google access token
    /// </summary>
    public string? GoogleAccessToken { get; set; }

    /// <summary>
    /// Google refresh token
    /// </summary>
    public string? GoogleRefreshToken { get; set; }

    /// <summary>
    /// Expiry time
    /// </summary>
    public DateTime GoogleTokenExpiry { get; set; }
}