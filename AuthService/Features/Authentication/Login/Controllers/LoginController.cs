using AuthService.Features.Authentication.Login.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Features.Authentication.Login.Controllers;

public static class LoginController
{
    public static Task LoginUser(this WebApplication app)
    {
        app.MapPost("/api/AuthService/AppUsers/login",
                async ([FromBody]  LoginRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var jwt = await mediator.Send(request, cancellationToken);
                    return new { jwt };
                })
            .WithName("LoginAppUser")
            .WithTags("AppUsers")
            .WithOpenApi();
        return Task.CompletedTask;
    }
}