using AuthService.Features.Authentication.Register.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Features.Authentication.Register.Controllers;

public static class RegistrationController
{
    /// <summary>
    /// Controller to register user
    /// </summary>
    /// <param name="app"></param>
    public static void RegisterUser(this WebApplication app)
    {
        app.MapPost("/api/AuthService/AppUsers/register",
                async ([FromBody]  RegisterRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var id = await mediator.Send(request, cancellationToken);
                    return id;
                })
            .WithName("RegisterAppUser")
            .WithTags("AppUsers")
            .WithOpenApi();
    }
}