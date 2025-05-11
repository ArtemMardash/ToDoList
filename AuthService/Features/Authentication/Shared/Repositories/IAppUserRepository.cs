
using AuthService.Core.Entities;

namespace AuthService.Features.Authentication.Shared.Repositories;

public interface IAppUserRepository
{
    /// <summary>
    /// Method to add a new user
    /// </summary>
    public Task<Guid> CreateUserAsync(AppUser user, CancellationToken cancellationToken);
    
    /// <summary>
    /// Method to get user by id
    /// </summary>
    public Task<AppUser?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Method to get user by email
    /// </summary>
    public Task<AppUser> GetUserByEmailAsyn(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Method to validate password
    /// </summary>
    public Task<bool> ValidatePasswordAsync(Guid id, string password, CancellationToken cancellationToken);

    /// <summary>
    /// Method to check if refresh token valid
    /// </summary>
    public Task<bool> CheckTokenAsync(Guid id, string currentRefreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Method to set a new refresh token
    /// </summary>
    public Task<string> SetRefreshTokenAsync(Guid id, string refreshToken, DateTime tokenExpirationDate,
        CancellationToken cancellationToken);
}