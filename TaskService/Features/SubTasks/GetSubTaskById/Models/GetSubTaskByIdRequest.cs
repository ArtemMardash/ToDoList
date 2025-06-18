using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;

namespace TaskService.Features.SubTasks.GetSubTaskById.Models;

public class GetSubTaskByIdRequest : IRequest<GetSubTaskByIdResult>
{
    public Guid Id { get; set; }
}

public class GetSubTaskByIdResult
{
    /// <summary>
    /// Id of sub task
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of subTask
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Status of subtask
    /// </summary>
    public TaskAndSubtaskStatus TaskStatus { get; set; }

    /// <summary>
    /// Original task 
    /// </summary>
    public Guid ParentId { get; set; }
}