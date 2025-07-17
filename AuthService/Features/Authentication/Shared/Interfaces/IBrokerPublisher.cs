using AuthService.Core.Events;

namespace AuthService.Features.Authentication.Shared.Interfaces;

public interface IBrokerPublisher
{
    public Task PublishGoogleLoginAsync(GoogleLogin googleLogin, CancellationToken cancellationToken);

    public Task PublishGoogleRegisteredAsync(GoogleRegistered googleRegistered, CancellationToken cancellationToken);
}