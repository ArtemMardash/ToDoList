using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.RefreshToken.Models;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Jwt;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Features.Authentication.RefreshToken.UserCases;

public class RefreshTokenHandler: IRequestHandler<RefreshTokenRequest, RefreshTokenResult>
{
    private readonly IJwtService _jwtService;
    private readonly IAppUserRepository _appUserRepository;

    public RefreshTokenHandler(IJwtService jwtService, IAppUserRepository appUserRepository)
    {
        _jwtService = jwtService;
        _appUserRepository = appUserRepository;
    }
    
    /// <summary>
    /// Method to refresh token
    /// </summary>
    public async ValueTask<RefreshTokenResult> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        ClaimsPrincipal refreshPrinciples;
        try
        {
            refreshPrinciples = _jwtService.ValidateToken(request.RefreshToken, true);
        }
        catch (SecurityTokenException securityTokenException)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        
        var userId = Guid.Parse(refreshPrinciples.FindFirstValue(ClaimTypes.NameIdentifier));
        if (userId == null)
        {
            throw new InvalidOperationException("Can't parse to Id");
        }
        var user = await _appUserRepository.GetUserByIdAsync(userId, cancellationToken);
        var refreshToken = _jwtService.GenerateRefreshToken(user);
        if (!await _appUserRepository.CheckTokenAsync(user.Id, refreshToken, cancellationToken))
        {
            throw new UnauthorizedAccessException("Access denied");
        }
        
        var newToken = await _appUserRepository.SetRefreshTokenAsync(user.Id, refreshToken, DateTime.Now.AddDays(7), cancellationToken);
        return new RefreshTokenResult
        {
            RefreshToken = newToken
        };
    }
}