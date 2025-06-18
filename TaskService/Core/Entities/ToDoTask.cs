using TaskService.Core.Enums;
using TaskService.Features;

namespace TaskService.Core.Entities;

public class ToDoTask
{
    /// <summary>
    /// Id of task
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id of user that wrote/have this task
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
    /// Deadline of the task
    /// </summary>
    public DateTime Deadline { get; set; }

    /// <summary>
    /// Status of the task
    /// </summary>
    public TaskAndSubtaskStatus TaskStatus { get; set; }

    /// <summary>
    /// Category of the task
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Subtasks for one main task
    /// </summary>
    public List<Subtask> SubTasks { get; set; }

    /// <summary>
    /// Constructor for creation
    /// </summary>
    public ToDoTask(Guid userId, string name, string description, Category category, DateTime deadline,
        TaskAndSubtaskStatus taskStatus, List<Subtask>? subTasks)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        Description = description;
        Category = category;
        Deadline = deadline;
        TaskStatus = taskStatus;
        SubTasks = subTasks ?? new List<Subtask>();
    }

    /// <summary>
    /// Constructor for mapping
    /// </summary>
    public ToDoTask(Guid id, Guid userId, string name, string description, Category category, DateTime deadline,
        TaskAndSubtaskStatus taskStatus, List<Subtask>? subTasks)
    {
        Id = id;
        Name = name;
        UserId = userId;
        Description = description;
        Category = category;
        Deadline = deadline;
        TaskStatus = taskStatus;
        SubTasks = subTasks ?? new List<Subtask>();
    }
}