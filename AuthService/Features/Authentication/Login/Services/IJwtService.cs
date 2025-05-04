using AuthService.Core.Entities;

namespace AuthService.Features.Authentication.Login.Services;

public interface IJwtService
{
    public string GenerateToken(AppUser appUser);

    public string GenerateRefreshToken(string jwtToken);
}