using FluentValidation;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;

namespace TaskService.Features.SubTasks.UpdateSubTask.Models;

public class UpdateSubTaskRequest : IRequest
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
    public ToDoTask Parent { get; set; }
}

public class UpdateSubtaskValidator : AbstractValidator<UpdateSubTaskRequest>
{
    public UpdateSubtaskValidator()
    {
        RuleFor(r => r.Name).MinimumLength(2);
    }
}