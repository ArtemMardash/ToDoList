using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;

namespace TaskService.Features.ToDoTasks.GetTaskById.Models;

public class GetTaskByIdRequest : IRequest<TaskDtoResult>
{
    public Guid Id { get; set; }
}

public class TaskDtoResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public CategoryResult Category { get; set; }

    public DateTime Deadline { get; set; }

    public DateTime Start { get; set; }
    
    public TaskAndSubtaskStatus TaskStatus { get; set; }

    public List<SubTaskResult> SubTasks { get; set; } = [];

    public class CategoryResult
    {
        /// <summary>
        /// Id of category
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of category
        /// </summary>
        public string Description { get; set; }
    }

    public class SubTaskResult
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
}