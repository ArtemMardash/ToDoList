using AuthService.Features.Authentication.Register.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Features.Authentication.Register.Controllers;

public static class RegistrationController
{
    public static Task RegisterUser(this WebApplication app)
    {
        app.MapGet("/api/AuthService/AppUsers/register",
                async ([FromBody]  RegisterRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var id = await mediator.Send(request, cancellationToken);
                    return new { id };
                })
            .WithName("RegisterAppUser")
            .WithTags("AppUsers")
            .WithOpenApi();
        return Task.CompletedTask;
    }
}