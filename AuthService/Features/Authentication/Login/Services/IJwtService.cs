using System.Security.Claims;
using AuthService.Core.Entities;

namespace AuthService.Features.Authentication.Login.Services;

public interface IJwtService
{
    public string GenerateAccessToken(AppUser appUser);

    public string GenerateRefreshToken(AppUser appUser);

    public ClaimsPrincipal ValidateToken(string token, bool isRefresh);
}