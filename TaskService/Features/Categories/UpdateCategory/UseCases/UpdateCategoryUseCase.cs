using FluentValidation;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Features.Categories.UpdateCategory.Models;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.SubTasks.UpdateSubTask.Models;
using TaskService.Infrastructure.Repositories;

namespace TaskService.Features.Categories.UpdateCategory.UseCases;

public class UpdateCategoryUseCase : IRequestHandler<UpdateCategoryRequest>
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateCategoryRequest> _validator;

    public UpdateCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork,
        IValidator<UpdateCategoryRequest> validator)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async ValueTask<Unit> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        await _categoryRepository.UpdateCategoryAsync(new Category(request.Id, request.Name, request.Description),
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}