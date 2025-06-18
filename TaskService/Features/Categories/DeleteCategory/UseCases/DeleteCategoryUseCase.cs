using FluentValidation;
using Mediator;
using TaskService.Features.Categories.DeleteCategory.Models;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;

namespace TaskService.Features.Categories.DeleteCategory.UseCases;

public class DeleteCategoryUseCase : IRequestHandler<DeleteCategoryRequest>
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly IUnitOfWork _unitOfWork;


    public DeleteCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Unit> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        await _categoryRepository.DeleteCategoryAsync(request.CategoryId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}