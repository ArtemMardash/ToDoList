using FluentValidation;
using FluentValidation.Validators;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.SubTasks.AddSubTask.Models;

namespace TaskService.Features.SubTasks.AddSubTask.UseCase;

public class AddSubTaskUseCase : IRequestHandler<AddSubTaskRequest, AddSubTaskResult>
{
    private readonly ITaskRepository _taskRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ISubTaskRepository _subTaskRepository;
    private readonly IValidator<AddSubTaskRequest> _validator;

    public AddSubTaskUseCase(ITaskRepository taskRepository, IUnitOfWork unitOfWork,
        ISubTaskRepository subTaskRepository, IValidator<AddSubTaskRequest> validator)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _subTaskRepository = subTaskRepository;
        _validator = validator;
    }

    public async ValueTask<AddSubTaskResult> Handle(AddSubTaskRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var task = await _taskRepository.GetTaskByIdAsync(request.ParentId, cancellationToken);
        var id = await _subTaskRepository.CreateSubTaskAsync(new Subtask(request.Name, request.TaskStatus, task),
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new AddSubTaskResult
        {
            Id = id
        };
    }
}