using System.ComponentModel.DataAnnotations;
using AuthService.Features.Authentication.Login.Models;
using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Jwt;
using FluentValidation;
using Mediator;

namespace AuthService.Features.Authentication.Login.UseCases;

public class LoginHandler: IRequestHandler<LoginRequest, LoginRequestResult>
{
    /// <summary>
    /// User repository to get user
    /// </summary>
    private readonly IAppUserRepository _appUserRepository;
    /// <summary>
    /// Validator of models
    /// </summary>
    private readonly IValidator<LoginRequest> _validator;
    /// <summary>
    /// Jwt service to create token
    /// </summary>
    private readonly IJwtService _jwtService;

    public LoginHandler(IAppUserRepository appUserRepository, IValidator<LoginRequest> validator, IJwtService jwtService)
    {
        _appUserRepository = appUserRepository;
        _validator = validator;
        _jwtService = jwtService;
    }
    
    /// <summary>
    /// Login handler
    /// </summary>
    public async ValueTask<LoginRequestResult> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _appUserRepository.GetUserByEmailAsyn(request.Email, cancellationToken);

        if (!await _appUserRepository.ValidatePasswordAsync(user.Id, request.Password, cancellationToken))
        {
            throw new InvalidOperationException("Wrong Password");
        }

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshTokenSet = _jwtService.GenerateRefreshToken(user);

        await _appUserRepository.SetRefreshTokenAsync(user.Id, refreshTokenSet.refreshToken, refreshTokenSet.expiriesAt,
            cancellationToken);
        
        return new LoginRequestResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenSet.refreshToken
        };

    }
}