using Mediator;

namespace AuthService.Features.Authentication.RefreshToken.Models;

public class RefreshTokenRequest : IRequest<RefreshTokenResult>
{
    public string RefreshToken { get; set; }
}

public class RefreshTokenResult
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}