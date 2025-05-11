using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Jwt;

public class JwtSettings
{
    public string Issuer { get; set; }

    public string Audience { get; set; }
    
    public TokenSettings AccessToken { get; set; }
    
    public TokenSettings RefreshToken { get; set; }
}

public class TokenSettings
{
    public string Key { get; set; }

    public int ExpiryInMinutes { get; set; }
}