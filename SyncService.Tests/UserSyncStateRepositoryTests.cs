using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Persistence;

namespace SyncService.Tests;

public class UserSyncStateRepositoryTests: IDisposable
{
    private readonly IUserSyncStateRepository _userSyncRepository;
    private readonly SyncDbContext _dbContext;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();
    private static Guid _userSyncId;
    
    public UserSyncStateRepositoryTests()
    {
        _userSyncRepository = _dbService.GetRequiredService<IUserSyncStateRepository>();
        _dbContext = _dbService.GetRequiredService<SyncDbContext>();
        _dbService.Migrate(_dbContext);
        _userSyncId = _dbService.CreateUser(_userSyncRepository, _dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
    
    [Fact]
    public async Task Create_User_Sync_State_Should_Success_Async()
    {
        var userSyncState = new UserSyncState(Guid.NewGuid(), 
            "another access token", 
            "another refresh token",
            "google id",
            DateTime.UtcNow.AddDays(3));
        var id = await _userSyncRepository.AddUserSyncStateAsync(userSyncState, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        var userByIdAsync = await _userSyncRepository.GetUserSyncStateAsync(id, CancellationToken.None);
        userByIdAsync.Should().NotBeNull();
        userSyncState.GoogleAccessToken.Should().Be(userSyncState.GoogleAccessToken);
        userSyncState.GoogleRefreshToken.Should().Be(userSyncState.GoogleRefreshToken);
    }

    [Fact]
    public async Task Get_User_Sync_State_By_Id_Should_Be_Successful()
    {
        var userSyncState = await _userSyncRepository.GetUserSyncStateAsync(_userSyncId, CancellationToken.None);

        await Verify(userSyncState);
    }

    [Fact]
    public async Task Update_User_Sync_State_Should_Be_Successful()
    {
        var userToUpdate = await _userSyncRepository.GetUserSyncStateAsync(_userSyncId, CancellationToken.None);
        userToUpdate.GoogleAccessToken = "updated access token";
        userToUpdate.GoogleRefreshToken = "updated refresh token";
        userToUpdate.TokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userSyncRepository.UpdateUserSyncStateAsync(userToUpdate, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var userToCheck =
            await _dbContext.UsersSyncState
                .FirstOrDefaultAsync(u => u.Id == _userSyncId, CancellationToken.None);
        await Verify(userToCheck);
    }


    [Fact]
    public async Task Delete_User_Sync_State_Should_Be_Successful()
    {
        await _userSyncRepository.DeleteUserSyncStateAsync(_userSyncId, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _dbContext.UsersSyncState.FirstOrDefault(us => us.Id == _userSyncId).Should().BeNull();
    }
}