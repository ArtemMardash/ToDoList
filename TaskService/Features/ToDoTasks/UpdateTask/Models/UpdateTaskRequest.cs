using FluentValidation;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features.ToDoTasks.AddTask.Models;

namespace TaskService.Features.ToDoTasks.UpdateTask.Models;

public class UpdateTaskRequest : IRequest
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public CategoryDto Category { get; set; }

    public DateTime Deadline { get; set; }

    public TaskAndSubtaskStatus TaskStatus { get; set; }

    public List<SubTaskDtoForUpdate> SubTasks { get; set; } = [];
}

public class SubTaskDtoForUpdate
{
    /// <summary>
    /// Id of subtask
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of subtask
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Status of subtask
    /// </summary>
    public int TaskStatus { get; set; }
}

public class UpdateTaskValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskValidator()
    {
        RuleFor(r => r.Name).MinimumLength(2);
        RuleFor(r => r.Description).MinimumLength(8);
        RuleFor(r => r.Category.Name).MinimumLength(2);
        RuleFor(r => r.Category.Description).MinimumLength(8);
        RuleFor(r => r.Deadline).GreaterThan(DateTime.Now);
    }
}