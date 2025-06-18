using AuthService.Core.Entities;
using AuthService.Core.ValueObjects;
using AuthService.Features.Authentication.ExternalAuth.Google.Models;
using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.Shared.Repositories;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Authentication;

namespace AuthService.Features.Authentication.ExternalAuth.Google.UseCases;

public class GoogleLoginHandler : IRequestHandler<GoogleLoginRequest, GoogleLoginResult>
{
    /// <summary>
    /// User repository to get user
    /// </summary>
    private readonly IAppUserRepository _appUserRepository;

    /// <summary>
    /// Validator of models
    /// </summary>
    private readonly IValidator<GoogleLoginRequest> _validator;

    /// <summary>
    /// Jwt service to create token
    /// </summary>
    private readonly IJwtService _jwtService;

    public GoogleLoginHandler(IAppUserRepository appUserRepository, IValidator<GoogleLoginRequest> validator,
        IJwtService jwtService)
    {
        _appUserRepository = appUserRepository;
        _validator = validator;
        _jwtService = jwtService;
    }

    public async ValueTask<GoogleLoginResult> Handle(GoogleLoginRequest request, CancellationToken cancellationToken)
    {
        AppUser user = null;
        try
        {
            user = await _appUserRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            var randForPass = new Random();
            var password = "Aa" + randForPass.Next(100000, 999999) + "!";
            var appUser = new AppUser(request.Email, password, new FullName(request.FirstName, request.LastName));
            var id = await _appUserRepository.CreateUserAsync(appUser, cancellationToken);
            user = appUser;
        }

        var refreshTokenSet = _jwtService.GenerateRefreshToken(user);
        await _appUserRepository.SetRefreshTokenAsync(user.Id, refreshTokenSet.refreshToken, refreshTokenSet.expiriesAt,
            cancellationToken);
        return new GoogleLoginResult
        {
            AccessToken = _jwtService.GenerateAccessToken(user),
            RefreshToken = refreshTokenSet.refreshToken
        };
    }
}