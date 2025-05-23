using System.Security.Claims;
using AuthService.Core.Entities;

namespace AuthService.Features.Authentication.Login.Services;

public interface IJwtService
{
    /// <summary>
    /// Method to generate access token
    /// </summary>
    public string GenerateAccessToken(AppUser appUser);

    /// <summary>
    /// Method to generate refresh token
    /// </summary>
    public (string refreshToken, DateTime expiriesAt) GenerateRefreshToken(AppUser appUser);

    /// <summary>
    /// Method to validate token
    /// </summary>
    public ClaimsPrincipal ValidateToken(string token, bool isRefresh);
}