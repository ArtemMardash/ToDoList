using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Core.Entities;
using AuthService.Features.Authentication.Login.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Jwt;

public class JwtService: IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Method to build token
    /// </summary>
    private string BuildToken(AppUser appUser, string secretKey, DateTime experation, IEnumerable<Claim> additionalClaims)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
            new Claim(JwtRegisteredClaimNames.Sub, appUser.Id.ToString()),
        };
        if (additionalClaims != null && additionalClaims.Any())
        {
            claims.AddRange(additionalClaims);
        }
        
        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: experation,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)), 
                SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// Method to generate access token
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    public string GenerateAccessToken(AppUser appUser)
    {
        return BuildToken(
            appUser,
            _jwtSettings.AccessToken.Key,
            DateTime.UtcNow.AddMinutes(_jwtSettings.AccessToken.ExpiryInMinutes),
            additionalClaims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("token_type", "access")
            }
        );
    }

    /// <summary>
    /// Method to generate refresh token
    /// </summary>
    public (string refreshToken, DateTime expiriesAt) GenerateRefreshToken(AppUser appUser)
    {
        var expiriesAt = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshToken.ExpiryInMinutes);
        var token = BuildToken(
            appUser,
            _jwtSettings.RefreshToken.Key,
            expiriesAt,
            additionalClaims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("token_type", "refresh")
            }
        );

        return (token, expiriesAt);
    }

    /// <summary>
    /// Method to validate token and get claims
    /// </summary>
    public ClaimsPrincipal ValidateToken(string token, bool isRefresh)
    {
        var key = Encoding.UTF8.GetBytes( isRefresh ? _jwtSettings.RefreshToken.Key : _jwtSettings.AccessToken.Key);
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.ValidateToken(token, validationParams, out _);
    }
}