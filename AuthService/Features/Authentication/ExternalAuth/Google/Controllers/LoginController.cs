using System.Security.Claims;
using AuthService.Features.Authentication.ExternalAuth.Google.Models;
using Mediator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
                        var accessToken = result.Properties.Items[".Token.access_token"];
                        _ =
                            result.Properties.Items.TryGetValue(".Token.refrsh_token", out var refreshToken);
                        _ = DateTime.TryParse(result.Properties.Items[".Token.expires_at"], out var tokenExpiry);
                        
                        var request = new GoogleLoginRequest
                        {
                            GoogleId = principal.FindFirstValue(ClaimTypes.NameIdentifier),
                            Email = principal.FindFirstValue(ClaimTypes.Email),
                            AccessToken = accessToken,
                            RefreshToken = refreshToken,
                            Expiry = tokenExpiry,
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