using Mediator;

namespace TaskService.Features.Categories.DeleteCategory.Models;

public class DeleteCategoryRequest : IRequest
{
    public Guid CategoryId { get; set; }
}