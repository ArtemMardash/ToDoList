using Microsoft.EntityFrameworkCore;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Mapping;

namespace SyncService.Infrastructure.Persistence.Repositories;

public class UserSyncStateRepository: IUserSyncStateRepository
{
    private readonly SyncDbContext _syncDbContext;

    public UserSyncStateRepository(SyncDbContext _syncDbContext)
    {
        this._syncDbContext = _syncDbContext;
    }
    
    public async Task<Guid> AddUserSyncStateAsync(UserSyncState userSyncState, CancellationToken cancellationToken)
    {
        await _syncDbContext.UsersSyncState.AddAsync(userSyncState.ToDb(), cancellationToken);

        return userSyncState.Id;
    }

    public async Task UpdateUserSyncStateAsync(UserSyncState userSyncState, CancellationToken cancellationToken)
    {
        var userSyncDb = await _syncDbContext
            .UsersSyncState
            .FirstOrDefaultAsync(u => u.Id == userSyncState.Id, cancellationToken);
        if(userSyncDb == null)
        {
            throw new InvalidOperationException("There is no user with such Id");
        }

        userSyncDb.GoogleAccessToken = userSyncState.GoogleAccessToken;
        userSyncDb.GoogleRefreshToken = userSyncState.GoogleRefreshToken;
        userSyncDb.TokenExpiry = userSyncState.TokenExpiry;
    }

    public async Task DeleteUserSyncStateAsync(Guid id, CancellationToken cancellationToken)
    {
        var userSyncDb = await _syncDbContext
            .UsersSyncState
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (userSyncDb == null)
        {
            throw new InvalidOperationException("There is no user with such Id");
        }

        _syncDbContext.UsersSyncState.Remove(userSyncDb);
    }

    public async Task<UserSyncState> GetUserSyncStateAsync(Guid id, CancellationToken cancellationToken)
    {
        var userSyncDb = await _syncDbContext
            .UsersSyncState
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (userSyncDb == null)
        {
            throw new InvalidOperationException("There is no user with such Id");
        }

        return userSyncDb.ToDomain();
    }

    public async Task<UserSyncState> GetUserSyncStateByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var userSyncDb = await _syncDbContext
                .UsersSyncState.
                FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        if (userSyncDb == null)
        {
            throw new InvalidOperationException("There is no user with such userId");
        }

        return userSyncDb.ToDomain();
    }
}