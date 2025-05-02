
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
    public Task<AppUser> GetUserByEmail(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Method to validate password
    /// </summary>
    public Task<bool> ValidatePasswordAsync(Guid id, string password, CancellationToken cancellationToken);
}