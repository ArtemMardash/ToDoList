using FluentValidation;
using Mediator;

namespace TaskService.Features.Categories.GetCategoryByName.Models;

public class GetCategoryByNameRequest : IRequest<GetCategoryByNameResult>
{
    public string Name { get; set; }
}

public class GetCategoryByNameResult
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}

public class GetCategoryRequestValidator : AbstractValidator<GetCategoryByNameRequest>
{
    public GetCategoryRequestValidator()
    {
        RuleFor(r => r.Name).MinimumLength(2);
    }
}