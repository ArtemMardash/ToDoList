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
    private readonly JwtService _jwtService;

    public LoginHandler(IAppUserRepository appUserRepository, IValidator<LoginRequest> validator, JwtService jwtService)
    {
        _appUserRepository = appUserRepository;
        _validator = validator;
        _jwtService = jwtService;
    }
    
    public async ValueTask<LoginRequestResult> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _appUserRepository.GetUserByEmail(request.Email, cancellationToken);

        if (!await _appUserRepository.ValidatePasswordAsync(user.Id, request.Password, cancellationToken))
        {
            throw new InvalidOperationException("Wrong Password");
        }

        var token = _jwtService.GenerateToken(user);
        return new LoginRequestResult
        {
            Token = token
        };

    }
}