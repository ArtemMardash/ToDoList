using System.ComponentModel.DataAnnotations;
using AuthService.Features.Authentication.Login.Models;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Jwt;
using FluentValidation;
using Mediator;

namespace AuthService.Features.Authentication.Login.UseCases;

public class LoginHandler: IRequestHandler<LoginRequest, LoginRequestResult>
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IValidator<LoginRequest> _validator;
    private readonly JwtUtils _jwtUtils;

    public LoginHandler(IAppUserRepository appUserRepository, IValidator<LoginRequest> validator, JwtUtils jwtUtils)
    {
        _appUserRepository = appUserRepository;
        _validator = validator;
        _jwtUtils = jwtUtils;
    }
    
    public async ValueTask<LoginRequestResult> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _appUserRepository.GetUserByEmail(request.Email, cancellationToken);

        if (!await _appUserRepository.ValidatePasswordAsync(user, request.Password, cancellationToken))
        {
            throw new InvalidOperationException("Wrong Password");
        }

        var token = _jwtUtils.GenerateToken(user.Email);
        return new LoginRequestResult
        {
            Jwt = token
        };

    }
}