namespace TaskService.Infrastructure.Persistence.Entities;

public class TaskDb: BaseEntity
{
    /// <summary>
    /// Id of task
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id of user
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Name of the task
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the task
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Category of the task
    /// </summary>
    public CategoryDb Category { get; set; }
    
    /// <summary>
    /// Begining of the task
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Deadline of the task
    /// </summary>
    public DateTime Deadline { get; set; }

    /// <summary>
    /// Status of the task
    /// </summary>
    public int TaskStatus { get; set; }

    /// <summary>
    /// Subtasks for one main task
    /// </summary>
    public virtual List<SubTaskDb> SubTasks { get; set; }
}