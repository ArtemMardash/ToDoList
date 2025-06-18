using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure;

namespace TaskService.Tests.RepositoriesTests;

public class SubTaskRepositoryTests : IDisposable
{
    private readonly ITaskRepository _taskRepository;
    private readonly ISubTaskRepository _subTaskRepository;
    private readonly TaskDbContext _dbContext;
    private static Guid _existedTaskId;
    private static Guid _existedSubTaskId;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();

    public SubTaskRepositoryTests()
    {
        _taskRepository = _dbService.GetRequiredService<ITaskRepository>();
        _subTaskRepository = _dbService.GetRequiredService<ISubTaskRepository>();
        _dbContext = _dbService.GetRequiredService<TaskDbContext>();
        _dbService.Migrate(_dbContext);
        _existedTaskId = _dbService.CreateTask(_taskRepository, _dbContext);
        _existedSubTaskId = _dbService.GetSubTaskId(_subTaskRepository, _taskRepository, _existedTaskId, _dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Create_Sub_Task_Should_Success_Async()
    {
        var task = await _taskRepository.GetTaskByIdAsync(_existedTaskId, CancellationToken.None);
        var subTask = new Subtask("NewSubtask", TaskAndSubtaskStatus.New, task);
        var id = await _subTaskRepository.CreateSubTaskAsync(subTask, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        var subTaskToCheck = await _subTaskRepository.GetSubTaskByIdAsync(id, CancellationToken.None);
        subTaskToCheck.Should().NotBeNull();
        subTaskToCheck.Name.Should().Be(subTask.Name);
        subTaskToCheck.TaskStatus.Should().Be(subTask.TaskStatus);
    }

    [Fact]
    public async Task Get_Sub_Task_By_Id_Should_Be_Successful()
    {
        var subTask = await _subTaskRepository.GetSubTaskByIdAsync(_existedSubTaskId, CancellationToken.None);

        await Verify(subTask);
    }

    [Fact]
    public async Task Update_Sub_Task_Should_Be_Successfull()
    {
        var subTaskToUpdate = await _subTaskRepository.GetSubTaskByIdAsync(_existedSubTaskId, CancellationToken.None);
        subTaskToUpdate.TaskStatus = TaskAndSubtaskStatus.InProgress;
        subTaskToUpdate.Name = "New super name";
        await _subTaskRepository.UpdateSubTaskAsync(subTaskToUpdate, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var subTaskToCheck =
            await _dbContext.Subtasks
                .FirstOrDefaultAsync(ts => ts.Id == _existedSubTaskId, CancellationToken.None);
        await Verify(subTaskToCheck);
    }


    [Fact]
    public async Task Delete_Task_Should_Be_Successful()
    {
        await _subTaskRepository.DeleteSubTaskAsync(_existedSubTaskId, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _dbContext.Subtasks.FirstOrDefault(st => st.Id == _existedSubTaskId).Should().BeNull();
    }
}