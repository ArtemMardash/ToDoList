using SyncService.Core.Entities;

namespace SyncService.Features.Shared.Repositories;

public interface IUserSyncStateRepository
{
    /// <summary>
    /// Method to add new user sync state
    /// </summary>
    public Task<Guid> AddUserSyncStateAsync(UserSyncState userSyncState, CancellationToken cancellationToken);

    /// <summary>
    /// Method to update user sync state
    /// </summary>
    public Task UpdateUserSyncStateAsync(UserSyncState userSyncState, CancellationToken cancellationToken);

    /// <summary>
    /// Method to delete user sync state
    /// </summary>
    public Task DeleteUserSyncStateAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Method to get user sync state by id
    /// </summary>
    public Task<UserSyncState> GetUserSyncStateAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Method to get user sync state by userId
    /// </summary>
    public Task<UserSyncState> GetUserSyncStateByUserId(Guid userId, CancellationToken cancellationToken);
}