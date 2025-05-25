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
        app.MapGet("/api/auth/google/callback",
                async (HttpContext context) =>
                {
                    var result = await context.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                    if (result.Succeeded)
                    {
                        context.Response.Redirect("/api/auth/google/profile");
                    }
                })
            .WithName("GoogleCallback")
            .WithTags("Google")
            .WithOpenApi();
        
        app.MapGet("/api/auth/google/login",
                async (HttpContext context) =>
                {
                    var props = new AuthenticationProperties
                    {
                        RedirectUri = "/api/auth/google/callback"
                    };
                    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, props);
                })
            .WithName("GoogleLogin")
            .WithTags("Google")
            .WithOpenApi();

        app.MapGet("/api/auth/google/profile", (ClaimsPrincipal user) =>
        {
            return user;
        }).RequireAuthorization();
    }
}