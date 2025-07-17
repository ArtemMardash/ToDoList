using FluentAssertions;
using Mediator;
using TaskService.Core.Enums;
using TaskService.Features.Categories.AddCategory.Models;
using TaskService.Features.Categories.DeleteCategory.Models;
using TaskService.Features.Categories.GetCategoryById.Models;
using TaskService.Features.Categories.GetCategoryByName.Models;
using TaskService.Features.Categories.UpdateCategory.Models;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.SubTasks.AddSubTask.Models;
using TaskService.Features.SubTasks.DeleteSubTask.Models;
using TaskService.Features.SubTasks.GetSubTaskById.Models;
using TaskService.Features.SubTasks.UpdateSubTask.Models;
using TaskService.Infrastructure;

namespace TaskService.Tests.SubTasksUseCaseTests;

public class SubtaskTests: IDisposable
{
    private readonly ISubTaskRepository _subTaskRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly TaskDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();
    private static Guid _subtaskId;
    private static Guid _taskId;

    public SubtaskTests()
    {
        _taskRepository = _dbService.GetRequiredService<ITaskRepository>();
        _subTaskRepository = _dbService.GetRequiredService<ISubTaskRepository>();
        _dbContext = _dbService.GetRequiredService<TaskDbContext>();
        _dbService.Migrate(_dbContext);
        _mediator = _dbService.GetRequiredService<IMediator>();
        _taskId = _dbService.CreateTask(_taskRepository, _dbContext);
        _subtaskId = _dbService.GetSubTaskId(_subTaskRepository, _taskRepository, _taskId, _dbContext);
    }

    [Fact]
    public async Task Create_Subtask_Should_Success_UseCase_Async()
    {
        var request = new AddSubTaskRequest()
        {
            Name = "New subtask for use case",
            TaskStatus = TaskAndSubtaskStatus.New,
            ParentId = _taskId
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    public async Task Create_Subtask_Should_Success_UseCase_Fail(string name)
    {
        var request = new AddSubTaskRequest()
        {
            Name = name,
            TaskStatus = TaskAndSubtaskStatus.New
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Delete_Subtask_Should_Success()
    {
        var request = new DeleteSubTaskRequest
        {
            Id = _subtaskId
        };

        await _mediator.Send(request, CancellationToken.None);
        _dbContext.Subtasks.FirstOrDefault(s => s.Id == _subtaskId).Should().BeNull();
    }

    [Theory]
    [InlineData("adda6bea-f80a-448e-92e4-d02ec95bf778")]
    public async Task Delete_Subtask_Should_Fail(string id)
    {
        var request = new DeleteSubTaskRequest()
        {
            Id = Guid.Parse(id)
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Get_Subtask_By_Id_Should_Success()
    {
        var request = new GetSubTaskByIdRequest()
        {
            Id = _subtaskId
        };

        var result = await _mediator.Send(request, CancellationToken.None);
        await Verify(result);
    }

    [Theory]
    [InlineData("adda6bea-f80a-448e-92e4-d02ec95bf778")]
    public async Task Get_Subtask_By_Id_Should_Fail(string id)
    {
        var request = new GetSubTaskByIdRequest()
        {
            Id = Guid.Parse(id)
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<InvalidOperationException>();
    }


    [Fact]
    public async Task Update_Subtask_Should_Success_UseCase_Async()
    {
        var request = new UpdateSubTaskRequest
        {
            Id = _subtaskId,
            Name = "new name forsubtask",
            TaskStatus = TaskAndSubtaskStatus.InProgress,
            Parent = await _taskRepository.GetTaskByIdAsync(_taskId, CancellationToken.None)
        };

        await _mediator.Send(request, CancellationToken.None);

        var result = _dbContext.Subtasks.FirstOrDefault(s => s.Id == _subtaskId);
        result!.Name.Should().Be("new name forsubtask");
        result.TaskStatus.Should().Be(2);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    public async Task Update_Subtask_Should_UseCase_Fail(string name)
    {
        var request = new UpdateSubTaskRequest
        {
            Id = _subtaskId,
            Name = name,
            TaskStatus = TaskAndSubtaskStatus.InProgress,
            Parent = await _taskRepository.GetTaskByIdAsync(_taskId, CancellationToken.None)
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}