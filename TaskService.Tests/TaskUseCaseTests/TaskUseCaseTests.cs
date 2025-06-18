using System.Globalization;
using FluentAssertions;
using Mediator;
using TaskService.Core.Enums;
using TaskService.Features.Categories.AddCategory.Models;
using TaskService.Features.Categories.DeleteCategory.Models;
using TaskService.Features.Categories.GetCategoryById.Models;
using TaskService.Features.Categories.GetCategoryByName.Models;
using TaskService.Features.Categories.UpdateCategory.Models;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.ToDoTasks.AddTask.Models;
using TaskService.Features.ToDoTasks.DeleteTask.Models;
using TaskService.Features.ToDoTasks.GetTaskById.Models;
using TaskService.Features.ToDoTasks.GetTasksByCategory.Models;
using TaskService.Features.ToDoTasks.UpdateTask.Models;
using TaskService.Infrastructure;

namespace TaskService.Tests.TaskUseCaseTests;

public class TaskUseCaseTests
{
    private readonly ITaskRepository _taskRepository;
    private readonly TaskDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();
    private static Guid _taskId;

    public TaskUseCaseTests()
    {
        _taskRepository = _dbService.GetRequiredService<ITaskRepository>();
        _dbContext = _dbService.GetRequiredService<TaskDbContext>();
        _dbService.Migrate(_dbContext);
        _mediator = _dbService.GetRequiredService<IMediator>();
        _taskId = _dbService.CreateTask(_taskRepository, _dbContext);
    }

    [Fact]
    public async Task Create_ToDoTask_Should_Success_UseCase_Async()
    {
        var request = new AddTaskRequest
        {
            UserId = Guid.Parse("b57d3d21-1b25-49d8-92c0-275034d35942"),
            Name = "Read",
            Description = "Read 10 pages",
            Category = new CategoryDto
            {
                Name = "Books",
                Description = "new Description"
            },
            Deadline = DateTime.UtcNow.AddDays(7),
            TaskStatus = TaskAndSubtaskStatus.New,
            SubTasks = new List<SubTaskDto>()
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("", "asdsdcsdacsdcsd")]
    [InlineData("ascsdcsdcsd", "")]
    [InlineData("", "")]
    public async Task Create_Task_Should_Fail(string name, string description)
    {
        var request = new AddTaskRequest
        {
            UserId = Guid.Parse("b57d3d21-1b25-49d8-92c0-275034d35942"),
            Name = name,
            Description = description,
            Category = new CategoryDto
            {
                Name = "Books",
                Description = "new Description"
            },
            Deadline = DateTime.Now.AddDays(7),
            TaskStatus = TaskAndSubtaskStatus.New,
            SubTasks = new List<SubTaskDto>()
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Delete_Task_Should_Success()
    {
        var request = new DeleteTaskRequest
        {
            Id = _taskId
        };

        await _mediator.Send(request, CancellationToken.None);
        _dbContext.Tasks.FirstOrDefault(t => t.Id == _taskId).Should().BeNull();
    }

    [Theory]
    [InlineData("adda6bea-f80a-448e-92e4-d02ec95bf778")]
    public async Task Delete_Task_Should_Fail(string id)
    {
        var request = new DeleteTaskRequest()
        {
            Id = Guid.Parse(id)
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Get_Task_By_Id_Should_Success()
    {
        var request = new GetTaskByIdRequest()
        {
            Id = _taskId
        };

        var result = await _mediator.Send(request, CancellationToken.None);
        await Verify(result);
    }

    [Theory]
    [InlineData("adda6bea-f80a-448e-92e4-d02ec95bf778")]
    public async Task Get_Task_By_Id_Should_Fail(string id)
    {
        var request = new GetTaskByIdRequest()
        {
            Id = Guid.Parse(id)
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Get_Task_By_Category_Should_Success()
    {
        var task = await _taskRepository.GetTaskByIdAsync(_taskId, CancellationToken.None);
        var request = new GetTasksByCategoryRequest
        {
            CategoryId = task.Category.Id
        };

        var result = await _mediator.Send(request, CancellationToken.None);
        await Verify(result);
    }

    [Theory]
    [InlineData("adda6bea-f80a-448e-92e4-d02ec95bf778")]
    public async Task Get_Task_By_Category_Should_Fail(string id)
    {
        var request = new GetTasksByCategoryRequest
        {
            CategoryId = Guid.Parse(id)
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Tasks.Count.Should().Be(0);
    }

    [Fact]
    public async Task Update_Task_Should_Success_UseCase_Async()
    {
        var request = new UpdateTaskRequest
        {
            Id = _taskId,
            UserId = Guid.Parse("b57d3d21-1b25-49d8-92c0-275034d35942"),
            Name = "Read a book",
            Description = "Read 20 pages",
            Category = new CategoryDto
            {
                Name = "Books",
                Description = "new Description"
            },
            Deadline = DateTime.UtcNow.AddDays(7),
            TaskStatus = TaskAndSubtaskStatus.New,
            SubTasks = new List<SubTaskDtoForUpdate>()
        };

        await _mediator.Send(request, CancellationToken.None);

        var result = _dbContext.Tasks.FirstOrDefault(t => t.Id == _taskId);
        result!.Name.Should().Be("Read a book");
        result.Description.Should().Be("Read 20 pages");
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("asdsdsscd", "")]
    [InlineData("", "dscdsccaca")]
    public async Task Update_Task_Should_Fail_UseCase_Async(string newName, string newDescription)
    {
        var request = new UpdateTaskRequest
        {
            Id = _taskId,
            UserId = Guid.Parse("b57d3d21-1b25-49d8-92c0-275034d35942"),
            Name = newName,
            Description = newDescription,
            Category = new CategoryDto
            {
                Name = "Books",
                Description = "new Description"
            },
            Deadline = DateTime.UtcNow.AddDays(7),
            TaskStatus = TaskAndSubtaskStatus.New,
            SubTasks = new List<SubTaskDtoForUpdate>()
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>();
    }
}