using Mediator;

namespace TaskService.Features.Categories.GetCategoryById.Models;

public class GetCategoryByIdRequest : IRequest<GetCategoryByIdResult>
{
    public Guid Id { get; set; }
}

public class GetCategoryByIdResult
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}