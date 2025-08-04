using FluentValidation;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.ToDoTasks.AddTask.Models;

namespace TaskService.Features.ToDoTasks.AddTask.UseCase;

public class AddTaskUseCase : IRequestHandler<AddTaskRequest, AddTaskResult>
{
    private readonly ITaskRepository _taskRepository;

    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<AddTaskRequest> _validator;

    private readonly IUnitOfWork _unitOfWork;

    public AddTaskUseCase(ITaskRepository taskRepository, IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository, IValidator<AddTaskRequest> validator)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async ValueTask<AddTaskResult> Handle(AddTaskRequest request, CancellationToken cancellationToken)
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

        var task = new ToDoTask(request.UserId, request.Name, request.Description, category, request.Start, request.Deadline,
            request.TaskStatus, new List<Subtask>());
        task.SubTasks = request.SubTasks.Select(st => ToSubTask(st, task)).ToList();
        var result = await _taskRepository.CreateTaskAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new AddTaskResult
        {
            Id = result
        };

        Subtask ToSubTask(SubTaskDto dto, ToDoTask task)
        {
            return new Subtask(dto.Name, dto.TaskStatus, task);
        }
    }
}