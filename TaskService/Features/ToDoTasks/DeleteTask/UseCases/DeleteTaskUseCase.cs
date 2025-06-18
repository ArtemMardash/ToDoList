using Mediator;
using TaskService.Features.Categories.DeleteCategory.Models;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.ToDoTasks.DeleteTask.Models;

namespace TaskService.Features.ToDoTasks.DeleteTask.UseCases;

public class DeleteTaskUseCase : IRequestHandler<DeleteTaskRequest>
{
    private readonly ITaskRepository _taskRepository;

    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskUseCase(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Unit> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        await _taskRepository.DeleteTaskAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}