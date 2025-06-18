using FluentValidation;
using Mediator;
using TaskService.Features.Categories.GetCategoryByName.Models;
using TaskService.Features.Shared.Repositories;

namespace TaskService.Features.Categories.GetCategoryByName.UseCases;

public class GetCategoryByNameUseCase : IRequestHandler<GetCategoryByNameRequest, GetCategoryByNameResult>
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly IValidator<GetCategoryByNameRequest> _validator;

    public GetCategoryByNameUseCase(ICategoryRepository categoryRepository,
        IValidator<GetCategoryByNameRequest> validator)
    {
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async ValueTask<GetCategoryByNameResult> Handle(GetCategoryByNameRequest request,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var result = await _categoryRepository.GetCategoryByNameAsync(request.Name, cancellationToken);
        return new GetCategoryByNameResult
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description
        };
    }
}