using FluentValidation;
using Mediator;

namespace AuthService.Features.Authentication.Login.Models;

public class LoginRequest: IRequest<LoginRequestResult>
{
    /// <summary>
    /// Email to get user
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Password to get access
    /// </summary>
    public string Password { get; set; }
}

public class LoginRequestResult
{
    /// <summary>
    /// AccessToken if it was generated
    /// </summary>
    public string AccessToken { get; set; }
    
    /// <summary>
    /// RefreshToken if it was generated
    /// </summary>
    public string RefreshToken { get; set; }
}

public class LoginValidator: AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Password).MinimumLength(8);
    }
}