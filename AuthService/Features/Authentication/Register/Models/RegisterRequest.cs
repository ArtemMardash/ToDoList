using FluentValidation;
using Mediator;

namespace AuthService.Features.Authentication.Register.Models;

public class RegisterRequest: IRequest<RegisterRequestResult>
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}

public class RegisterRequestResult
{
    public Guid Id { get; set; }
}

public class RegisterValidator: AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Password).MinimumLength(8);
        RuleFor(r => r.FirstName).MinimumLength(2).MaximumLength(30);
        RuleFor(r => r.LastName).MinimumLength(2).MaximumLength(40);
    }
}