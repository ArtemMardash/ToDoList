using AuthService.Core.Entities;
using AuthService.Core.Events;
using AuthService.Core.ValueObjects;
using AuthService.Features.Authentication.ExternalAuth.Google.Models;
using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.Shared.Interfaces;
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

    private readonly IBrokerPublisher _brokerPublisher;

    public GoogleLoginHandler(IAppUserRepository appUserRepository, IValidator<GoogleLoginRequest> validator,
        IJwtService jwtService, IBrokerPublisher brokerPublisher)
    {
        _appUserRepository = appUserRepository;
        _validator = validator;
        _jwtService = jwtService;
        _brokerPublisher = brokerPublisher;
    }

    public async ValueTask<GoogleLoginResult> Handle(GoogleLoginRequest request, CancellationToken cancellationToken)
    {
        AppUser user = null;
        var isNewUser = false;
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
            isNewUser = true;
        }

        var refreshTokenSet = _jwtService.GenerateRefreshToken(user);
        await _appUserRepository.SetRefreshTokenAsync(user.Id, refreshTokenSet.refreshToken, refreshTokenSet.expiriesAt,
            cancellationToken);

        var accessToken = _jwtService.GenerateAccessToken(user);

        if (isNewUser)
        {
            var googleRegistered = new GoogleRegistered
            {
                Id = user.Id,
                GoogleId = Guid.NewGuid().ToString(),
                GoogleRefreshToken = refreshTokenSet.refreshToken,
                GoogleAccessToken = accessToken,
                TokenExpiry = refreshTokenSet.expiriesAt
            };
            await _brokerPublisher.PublishGoogleRegisteredAsync(googleRegistered, cancellationToken);
        }
        else
        {
            var googleLogin = new GoogleLogin
            {
                UserId = user.Id,
                GoogleId = Guid.NewGuid().ToString(),
                GoogleRefreshToken = refreshTokenSet.refreshToken,
                GoogleAccessToken = accessToken,
                TokenExpiry = refreshTokenSet.expiriesAt
            };
            await _brokerPublisher.PublishGoogleLoginAsync(googleLogin, cancellationToken);
        }

        return new GoogleLoginResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenSet.refreshToken
        };
    }
}