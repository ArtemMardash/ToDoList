using FluentValidation;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;

namespace TaskService.Features.SubTasks.AddSubTask.Models;

public class AddSubTaskRequest : IRequest<AddSubTaskResult>
{
    /// <summary>
    /// Name of subtask
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

public class AddSubTaskResult
{
    public Guid Id { get; set; }
}

public class AddSubTaskRequestValidator : AbstractValidator<AddSubTaskRequest>
{
    public AddSubTaskRequestValidator()
    {
        RuleFor(r => r.Name).MinimumLength(2);
    }
}