using FluentValidation;
using Mediator;

namespace AuthService.Features.Authentication.Login.Models;

public class LoginRequest: IRequest<LoginRequestResult>
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}

public class LoginRequestResult
{
    public string Jwt { get; set; }
}

public class RegisterValidator: AbstractValidator<LoginRequest>
{
    public RegisterValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Password).MinimumLength(8);
    }
}