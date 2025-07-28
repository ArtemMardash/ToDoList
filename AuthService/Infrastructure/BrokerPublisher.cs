using AuthService.Core.Events;
using AuthService.Features.Authentication.Shared.Interfaces;
using MassTransit;
using Mediator;
using SharedKernel;

namespace AuthService.Infrastructure;

public class BrokerPublisher: IBrokerPublisher
{
    private readonly IPublishEndpoint _publisher;

    public BrokerPublisher(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }
    public async Task PublishGoogleLoginAsync(GoogleLogin googleLogin, CancellationToken cancellationToken)
    {
        await _publisher.Publish<IGoogleLogin>(new
        {
            UserId = googleLogin.UserId,
            GoogleId = googleLogin.GoogleId,
            RefreshToken = googleLogin.GoogleRefreshToken,
            AccessToken = googleLogin.GoogleAccessToken,
            TokenExpiry = googleLogin.TokenExpiry
        }, cancellationToken);
    }

    public async Task PublishGoogleRegisteredAsync(GoogleRegistered googleRegistered, CancellationToken cancellationToken)
    {
        await _publisher.Publish<IGoogleRegistered>(new
        {
            UserId = googleRegistered.Id,
            GoogleId = googleRegistered.GoogleId,
            GoogleRefreshToken = googleRegistered.GoogleRefreshToken,
            GoogleAccessToken = googleRegistered.GoogleAccessToken,
            TokenExpiry = googleRegistered.TokenExpiry
        }, cancellationToken);
    }
}