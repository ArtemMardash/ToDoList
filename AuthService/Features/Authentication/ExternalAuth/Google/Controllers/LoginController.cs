using System.Security.Claims;
using AuthService.Features.Authentication.ExternalAuth.Google.Models;
using Mediator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Features.Authentication.ExternalAuth.Google.Controllers;

public static class LoginController
{
    /// <summary>
    /// Google Login Controller
    /// </summary>
    public static void GoogleLoginUser(this WebApplication app)
    {
        app.MapGet("/api/auth/google/signin",
                async (HttpContext context, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await context.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                    if (result.Succeeded)
                    {
                        var principal = result.Principal;
                        var request = new GoogleLoginRequest
                        {
                            Email = principal.FindFirstValue(ClaimTypes.Email),
                            AccessToken = null,
                            RefreshToken = null,
                            Expiry = default,
                            FirstName = principal.FindFirstValue(ClaimTypes.Name),
                            LastName = principal.FindFirstValue(ClaimTypes.Surname)
                        };

                        return Results.Ok(await mediator.Send(request, cancellationToken));
                    }

                    return Results.Unauthorized();
                })
            .WithName("GoogleCallback")
            .WithTags("Google")
            .WithOpenApi();


        app.MapGet("/api/auth/google/login",
                async (HttpContext context) =>
                {
                    var props = new AuthenticationProperties
                    {
                        RedirectUri = "/api/auth/google/signin"
                    };
                    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, props);
                })
            .WithName("GoogleLogin")
            .WithTags("Google")
            .WithOpenApi();

        app.MapGet("/api/auth/google/profile", (ClaimsPrincipal user) => { return user; }).RequireAuthorization();
    }
}