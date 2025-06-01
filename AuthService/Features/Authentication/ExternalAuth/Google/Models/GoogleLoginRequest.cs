namespace AuthService.Features.Authentication.ExternalAuth.Google.Models;

public class GoogleLoginRequest
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