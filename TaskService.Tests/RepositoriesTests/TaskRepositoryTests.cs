using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure;

namespace TaskService.Tests.RepositoriesTests;

public class TaskRepositoryTests : IDisposable
{
    private readonly ITaskRepository _taskRepository;
    private readonly TaskDbContext _dbContext;
    private static Guid _existedTaskId;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();

    public TaskRepositoryTests()
    {
        _taskRepository = _dbService.GetRequiredService<ITaskRepository>();
        _dbContext = _dbService.GetRequiredService<TaskDbContext>();
        _dbService.Migrate(_dbContext);
        _existedTaskId = _dbService.CreateTask(_taskRepository, _dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task Create_Task_Should_Success_Async()
    {
        var task = new ToDoTask(Guid.Parse("b57d3d21-1b25-49d8-92c0-275034d35942"),
            "Pull Ups",
            "Do 10 pull ups",
            new Category("Sport", "Something involved exercising"),
            DateTime.UtcNow.AddDays(7),
            TaskAndSubtaskStatus.New,
            new List<Subtask>());
        var id = await _taskRepository.CreateTaskAsync(task, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        var taskToCheck = await _taskRepository.GetTaskByIdAsync(id, CancellationToken.None);
        taskToCheck.Should().NotBeNull();
        taskToCheck.Name.Should().Be(task.Name);
        taskToCheck.Description.Should().Be(task.Description);
        taskToCheck.Deadline.Should().Be(task.Deadline);
        taskToCheck.TaskStatus.Should().Be(task.TaskStatus);
    }

    [Fact]
    public async Task Get_Task_By_Id_Should_Be_Successful()
    {
        var toDoTask = await _taskRepository.GetTaskByIdAsync(_existedTaskId, CancellationToken.None);

        await Verify(toDoTask);
    }

    [Fact]
    public async Task Get_Task_By_Category_Should_Be_Successful()
    {
        var toDoTask = await _taskRepository.GetTaskByIdAsync(_existedTaskId, CancellationToken.None);

        var category = toDoTask.Category;

        var toDoTaskToCheck = await _taskRepository.GetTasksByCategoryAsync(category.Id, CancellationToken.None);

        await Verify(toDoTaskToCheck);
    }

    [Fact]
    public async Task UpdateTask_Should_Be_Successfull()
    {
        var taskToUpdate = await _taskRepository.GetTaskByIdAsync(_existedTaskId, CancellationToken.None);
        taskToUpdate.TaskStatus = TaskAndSubtaskStatus.InProgress;
        taskToUpdate.Name = "New super name";
        taskToUpdate.Description = "New super description";
        taskToUpdate.Deadline = DateTime.UtcNow.AddDays(7);
        await _taskRepository.UpdateTaskAsync(taskToUpdate, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var taskToCheck =
            await _dbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == _existedTaskId, CancellationToken.None);
        await Verify(taskToCheck);
    }

    [Fact]
    public async Task Delete_Task_Should_Be_Successful()
    {
        await _taskRepository.DeleteTaskAsync(_existedTaskId, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _dbContext.Tasks.Should().BeNullOrEmpty();
    }
}