using Mediator;
using Microsoft.AspNetCore.Mvc;
using TaskService.Features.SubTasks.AddSubTask.Models;
using TaskService.Features.SubTasks.DeleteSubTask.Models;
using TaskService.Features.SubTasks.GetSubTaskById.Models;
using TaskService.Features.SubTasks.UpdateSubTask.Models;

namespace TaskService.Features.SubTasks;

public static class SubTasksController
{
    public static void MapSubTaskEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/subtask")
            .RequireAuthorization()
            .WithTags("subtask");

        group.MapPost("/add",
                async (AddSubTaskRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken);
                    return result;
                })
            .WithName("AddSubTask")
            .WithOpenApi();

        group.MapDelete("/delete",
                async ([FromBody] DeleteSubTaskRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    await mediator.Send(request, cancellationToken);
                })
            .WithName("DeleteSubTask")
            .WithOpenApi();

        group.MapPut("/",
                async (UpdateSubTaskRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    await mediator.Send(request, cancellationToken);
                })
            .WithName("UpdateSubTask")
            .WithOpenApi();

        group.MapGet("/get/{id:guid}",
                async (Guid id, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var request = new GetSubTaskByIdRequest()
                    {
                        Id = id
                    };
                    var result = await mediator.Send(request, cancellationToken);
                    return result;
                })
            .WithName("GetSubTaskById")
            .WithOpenApi();
    }
}