using FluentValidation;
using Mediator;

namespace TaskService.Features.Categories.AddCategory.Models;

public class AddCategoryRequest : IRequest<AddCategoryResult>
{
    /// <summary>
    /// Name of the category
    /// </summary>
    public string Name { get; set; }

    public string Description { get; set; }
}

public class AddCategoryResult
{
    public Guid Id { get; set; }
}

public class AddCategoryValidator : AbstractValidator<AddCategoryRequest>
{
    public AddCategoryValidator()
    {
        RuleFor(r => r.Name).MinimumLength(2);
        RuleFor(r => r.Description).MinimumLength(5);
    }
}