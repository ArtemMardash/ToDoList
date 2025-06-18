using Mediator;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.SubTasks.DeleteSubTask.Models;
using TaskService.Features.ToDoTasks.DeleteTask.Models;

namespace TaskService.Features.SubTasks.DeleteSubTask.UseCase;

public class DeleteSubTaskUseCase : IRequestHandler<DeleteSubTaskRequest>
{
    private readonly ISubTaskRepository _subTaskRepository;

    private readonly IUnitOfWork _unitOfWork;

    public DeleteSubTaskUseCase(ISubTaskRepository subTaskRepository, IUnitOfWork unitOfWork)
    {
        _subTaskRepository = subTaskRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Unit> Handle(DeleteSubTaskRequest request, CancellationToken cancellationToken)
    {
        await _subTaskRepository.DeleteSubTaskAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}