using AuthService.Features.Authentication.RefreshToken.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Features.Authentication.RefreshToken.Controllers;

public static class RefreshTokenController
{
    /// <summary>
    /// Controller to refresh token
    /// </summary>
    /// <param name="app"></param>
    public static void RefreshToken(this WebApplication app)
    {
        app.MapPost("/api/AuthService/Token/Refresh",
                async ([FromBody]  RefreshTokenRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var jwt = await mediator.Send(request, cancellationToken);
                    return jwt;
                })
            .WithName("RefreshToken")
            .WithTags("AppUsers")
            .WithOpenApi();
    }
}