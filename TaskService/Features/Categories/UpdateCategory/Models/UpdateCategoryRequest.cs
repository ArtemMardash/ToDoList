using FluentValidation;
using Mediator;

namespace TaskService.Features.Categories.UpdateCategory.Models;

public class UpdateCategoryRequest : IRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(r => r.Name).MinimumLength(2);
        RuleFor(r => r.Description).MinimumLength(5);
    }
}