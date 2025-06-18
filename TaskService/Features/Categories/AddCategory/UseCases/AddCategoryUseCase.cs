using FluentValidation;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Features.Categories.AddCategory.Models;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure.Repositories;

namespace TaskService.Features.Categories.AddCategory.UseCases;

public class AddCategoryUseCase : IRequestHandler<AddCategoryRequest, AddCategoryResult>
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IValidator<AddCategoryRequest> _validator;

    public AddCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork,
        IValidator<AddCategoryRequest> validator)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async ValueTask<AddCategoryResult> Handle(AddCategoryRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        Category? existedCategory = default;
        try
        {
            existedCategory = await _categoryRepository.GetCategoryByNameAsync(request.Name, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
        }

        if (existedCategory != null)
        {
            throw new InvalidOperationException("The category with that name already exists");
        }

        var result =
            await _categoryRepository.CreateCategoryAsync(new Category(request.Name, request.Description),
                cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new AddCategoryResult
        {
            Id = result
        };
    }
}