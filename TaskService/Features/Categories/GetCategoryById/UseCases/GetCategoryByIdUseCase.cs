using Mediator;
using TaskService.Features.Categories.GetCategoryById.Models;
using TaskService.Features.Shared.Repositories;

namespace TaskService.Features.Categories.GetCategoryById.UseCases;

public class GetCategoryByIdUseCase : IRequestHandler<GetCategoryByIdRequest, GetCategoryByIdResult>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async ValueTask<GetCategoryByIdResult> Handle(GetCategoryByIdRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetCategoryByIdAsync(request.Id, cancellationToken);
        return new GetCategoryByIdResult
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description
        };
    }
}