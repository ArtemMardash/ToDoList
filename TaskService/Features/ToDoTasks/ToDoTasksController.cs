using Mediator;
using Microsoft.AspNetCore.Mvc;
using TaskService.Features.ToDoTasks.AddTask.Models;
using TaskService.Features.ToDoTasks.DeleteTask.Models;
using TaskService.Features.ToDoTasks.GetTaskById.Models;
using TaskService.Features.ToDoTasks.GetTasksByCategory.Models;
using TaskService.Features.ToDoTasks.UpdateTask.Models;

namespace TaskService.Features.ToDoTasks;

public static class ToDoTasksController
{
    public static void MapToDoTaskEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/task")
            .WithTags("todotask");

        group.MapPost("/add",
                async (AddTaskRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken);
                    return result;
                })
            .WithName("AddTask")
            .WithOpenApi();

        group.MapDelete("/delete",
                async ([FromBody] DeleteTaskRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    await mediator.Send(request, cancellationToken);
                })
            .WithName("DeleteTask")
            .WithOpenApi();

        group.MapGet("/get/{id:guid}",
                async (Guid id, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var request = new GetTaskByIdRequest
                    {
                        Id = id
                    };
                    var result = await mediator.Send(request, cancellationToken);
                    return result;
                })
            .WithName("GetTaskById")
            .WithOpenApi();

        group.MapGet("/get/by-category/{categoryId:guid}",
                async (Guid categoryId, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var request = new GetTasksByCategoryRequest
                    {
                        CategoryId = categoryId
                    };
                    var result = await mediator.Send(request, cancellationToken);
                    return result;
                })
            .WithName("GetTasksByCategory")
            .WithOpenApi();

        group.MapPut("/",
                async (UpdateTaskRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    await mediator.Send(request, cancellationToken);
                })
            .WithName("UpdateTask")
            .WithOpenApi();
    }
}