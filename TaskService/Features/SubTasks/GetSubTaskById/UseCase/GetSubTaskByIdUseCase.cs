using Mediator;
using TaskService.Core.Enums;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.SubTasks.GetSubTaskById.Models;

namespace TaskService.Features.SubTasks.GetSubTaskById.UseCase;

public class GetSubTaskByIdUseCase : IRequestHandler<GetSubTaskByIdRequest, GetSubTaskByIdResult>
{
    private readonly ISubTaskRepository _subTaskRepository;

    public GetSubTaskByIdUseCase(ISubTaskRepository subTaskRepository)
    {
        _subTaskRepository = subTaskRepository;
    }

    public async ValueTask<GetSubTaskByIdResult> Handle(GetSubTaskByIdRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _subTaskRepository.GetSubTaskByIdAsync(request.Id, cancellationToken);
        return new GetSubTaskByIdResult
        {
            Id = result.Id,
            Name = result.Name,
            TaskStatus = result.TaskStatus,
            ParentId = result.Parent.Id
        };
    }
}