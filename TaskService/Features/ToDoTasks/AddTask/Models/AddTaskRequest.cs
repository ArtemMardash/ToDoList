using FluentValidation;
using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;

namespace TaskService.Features.ToDoTasks.AddTask.Models;

public class AddTaskRequest : IRequest<AddTaskResult>
{
    public Guid UserId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public CategoryDto Category { get; set; }

    public DateTime Deadline { get; set; }

    public TaskAndSubtaskStatus TaskStatus { get; set; }

    public List<SubTaskDto> SubTasks { get; set; } = [];
}

public class CategoryDto
{
    /// <summary>
    /// Name of category
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of category
    /// </summary>
    public string Description { get; set; }
}

public class SubTaskDto
{
    /// <summary>
    /// Name of subtask
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Status of subtask
    /// </summary>
    public TaskAndSubtaskStatus TaskStatus { get; set; }
}

public class AddTaskResult
{
    public Guid Id { get; set; }
}

public class AddTaskValidator : AbstractValidator<AddTaskRequest>
{
    public AddTaskValidator()
    {
        RuleFor(r => r.Name).MinimumLength(2);
        RuleFor(r => r.Description).MinimumLength(8);
        RuleFor(r => r.Category.Name).MinimumLength(2);
        RuleFor(r => r.Category.Description).MinimumLength(8);
        RuleFor(r => r.Deadline).GreaterThan(DateTime.Now);
    }
}