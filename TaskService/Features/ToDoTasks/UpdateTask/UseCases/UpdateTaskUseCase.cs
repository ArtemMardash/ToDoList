using FluentValidation;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.ToDoTasks.UpdateTask.Models;

namespace TaskService.Features.ToDoTasks.UpdateTask.UseCases;

public class UpdateTaskUseCase : IRequestHandler<UpdateTaskRequest>
{
    private readonly ITaskRepository _taskRepository;

    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<UpdateTaskRequest> _validator;

    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskUseCase(ITaskRepository taskRepository, IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository, IValidator<UpdateTaskRequest> validator)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async ValueTask<Unit> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        Category category;
        try
        {
            category = await _categoryRepository.GetCategoryByNameAsync(request.Category.Name, cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            category = new Category(request.Category.Name, request.Category.Description);
        }

        var toDoTask = new ToDoTask(request.Id, request.UserId, request.Name, request.Description, category,
            request.Deadline, request.TaskStatus, new List<Subtask>());
        toDoTask.SubTasks = request.SubTasks.Select(st => ToSubTask(st, toDoTask)).ToList();
        await _taskRepository.UpdateTaskAsync(toDoTask, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;

        Subtask ToSubTask(SubTaskDtoForUpdate dto, ToDoTask parent)
        {
            return new Subtask(dto.Id, dto.Name, (TaskAndSubtaskStatus)dto.TaskStatus, parent);
        }
    }
}