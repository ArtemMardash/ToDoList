using FluentValidation;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.SubTasks.UpdateSubTask.Models;

namespace TaskService.Features.SubTasks.UpdateSubTask.UseCase;

public class UpdateSubTaskUseCase : IRequestHandler<UpdateSubTaskRequest>
{
    private readonly ISubTaskRepository _subTaskRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateSubTaskRequest> _validator;

    public UpdateSubTaskUseCase(ISubTaskRepository subTaskRepository, IUnitOfWork unitOfWork,
        IValidator<UpdateSubTaskRequest> validator)
    {
        _subTaskRepository = subTaskRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async ValueTask<Unit> Handle(UpdateSubTaskRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        await _subTaskRepository.UpdateSubTaskAsync(
            new Subtask(request.Id, request.Name, request.TaskStatus, request.Parent), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}