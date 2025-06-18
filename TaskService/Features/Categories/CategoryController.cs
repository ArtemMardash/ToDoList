using Mediator;
using Microsoft.AspNetCore.Mvc;
using TaskService.Features.Categories.AddCategory.Models;
using TaskService.Features.Categories.DeleteCategory.Models;
using TaskService.Features.Categories.GetCategoryById.Models;
using TaskService.Features.Categories.GetCategoryByName.Models;
using TaskService.Features.Categories.UpdateCategory.Models;

namespace TaskService.Features.Categories;

public static class CategoryController
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/category")
            .RequireAuthorization()
            .WithTags("category");


        group.MapPost("/",
                async (AddCategoryRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken);
                    return result;
                })
            .WithName("AddCategory")
            .WithOpenApi();

        group.MapDelete("/{id:guid}/delete",
                async (Guid id, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var request = new DeleteCategoryRequest
                    {
                        CategoryId = id
                    };
                    await mediator.Send(request, cancellationToken);
                })
            .WithName("DeleteCategory")
            .WithOpenApi();

        group.MapGet("/{id:guid}",
                async (Guid id, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var request = new GetCategoryByIdRequest
                    {
                        Id = id
                    };
                    var result = await mediator.Send(request, cancellationToken);
                    return result;
                })
            .WithName("GetCategoryById")
            .WithOpenApi();

        group.MapGet("/",
                async ([FromQuery] string name, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var request = new GetCategoryByNameRequest
                    {
                        Name = name
                    };
                    var result = await mediator.Send(request, cancellationToken);
                    return result;
                })
            .WithName("GetCategoryByName")
            .WithOpenApi();

        group.MapPut("/",
                async (UpdateCategoryRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    await mediator.Send(request, cancellationToken);
                })
            .WithName("UpdateCategory")
            .WithOpenApi();
    }
}