using TaskService.Core.Enums;
using TaskService.Features;

namespace TaskService.Core.Entities;

public class ToDoTask
{
    private const int NAME_MAX_LENGTH = 30;

    
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
    /// Start date of the task
    /// </summary>
    public DateTime Start { get; set; }

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
    public ToDoTask(Guid userId, string name, string description, Category category,DateTime start, DateTime deadline,
        TaskAndSubtaskStatus taskStatus, List<Subtask>? subTasks)
    {
        Id = Guid.NewGuid();
        SetName(name);
        UserId = userId;
        SetDescription(description);
        Category = category;
        Start = start;
        SetDeadLine(deadline);
        TaskStatus = taskStatus;
        SubTasks = subTasks ?? new List<Subtask>();
    }

    /// <summary>
    /// Constructor for mapping
    /// </summary>
    public ToDoTask(Guid id, Guid userId, string name, string description, Category category, DateTime start, DateTime deadline,
        TaskAndSubtaskStatus taskStatus, List<Subtask>? subTasks)
    {
        Id = id;
        SetName(name);
        UserId = userId;
        SetDescription(description);
        Category = category;
        Start = start;
        SetDeadLine(deadline);
        TaskStatus = taskStatus;
        SubTasks = subTasks ?? new List<Subtask>();
    }
    
    /// <summary>
    /// Method to set name
    /// </summary>
    private void SetName(string input)
    {
        if (input.Length > NAME_MAX_LENGTH || string.IsNullOrEmpty(input))
        {
            throw new InvalidOperationException("Invalid name of the ToDo task");
        }
        else
        {
            Name = input;
        }
    }

    /// <summary>
    /// Method to set description
    /// </summary>
    private void SetDescription(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new InvalidOperationException("Invalid description of the ToDo task");
        }
        else
        {
            Description = input;
        }
    }

    /// <summary>
    /// Method to set deadline
    /// </summary>
    private void SetDeadLine(DateTime input)
    {
        if (input.Date <= DateTime.Now)
        {
            throw new InvalidOperationException("The date can not be now or in the past");
        }
        else
        {
            Deadline = input;
        }
    }
}