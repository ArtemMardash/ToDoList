using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Persistence;

namespace SyncService.Tests;

public class TaskSyncMappingRepositoryTests: IDisposable
{
     private readonly ITaskSyncMappingRepository _taskSyncMappingRepository;
    private readonly SyncDbContext _dbContext;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();
    private static Guid _taskSyncId;
    
    public TaskSyncMappingRepositoryTests()
    {
        _taskSyncMappingRepository = _dbService.GetRequiredService<ITaskSyncMappingRepository>();
        _dbContext = _dbService.GetRequiredService<SyncDbContext>();
        _dbService.Migrate(_dbContext);
        _taskSyncId = _dbService.CreateTask(_taskSyncMappingRepository, _dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
    
    [Fact]
    public async Task Create_Task_Sync_Mapping_Should_Success_Async()
    {
        var taskSyncMapping = new TaskSyncMapping(Guid.NewGuid(), "Calendar event");
        var id = await _taskSyncMappingRepository.AddTaskSyncMappingAsync(taskSyncMapping, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        var taskById = await _taskSyncMappingRepository.GetTaskSyncMappingAsync(id, CancellationToken.None);
        taskById.Should().NotBeNull();
        taskById.CalendarEventId.Should().Be(taskSyncMapping.CalendarEventId);
        taskById.TaskId.Should().Be(taskSyncMapping.TaskId);
    }

    [Fact]
    public async Task Get_User_Sync_State_By_Id_Should_Be_Successful()
    {
        var taskSyncMappingAsync = await _taskSyncMappingRepository.GetTaskSyncMappingAsync(_taskSyncId, CancellationToken.None);

        await Verify(taskSyncMappingAsync);
    }
    


    [Fact]
    public async Task Delete_User_Sync_State_Should_Be_Successful()
    {
        await _taskSyncMappingRepository.DeleteTaskSyncMappingAsync(_taskSyncId, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _dbContext.UsersSyncState.FirstOrDefault(ts => ts.Id == _taskSyncId).Should().BeNull();
    }
}