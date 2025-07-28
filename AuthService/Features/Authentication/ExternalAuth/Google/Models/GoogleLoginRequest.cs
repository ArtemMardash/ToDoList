using FluentValidation;
using Mediator;

namespace AuthService.Features.Authentication.ExternalAuth.Google.Models;

public class GoogleLoginRequest : IRequest<GoogleLoginResult>
{
    public string GoogleId { get; set; }
    
    public string Email { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? Expiry { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

public class GoogleLoginResult
{
    /// <summary>
    /// AccessToken
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// RefreshToken 
    /// </summary>
    public string RefreshToken { get; set; }
}

public class GoogleLoginValidator : AbstractValidator<GoogleLoginRequest>
{
    public GoogleLoginValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.RefreshToken).NotEmpty();
        RuleFor(r => r.AccessToken).NotEmpty();
        RuleFor(r => r.Expiry).NotNull();
    }
}