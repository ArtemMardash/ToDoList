using TaskService.Core.Enums;

namespace TaskService.Core.Entities;

public class Subtask
{
    /// <summary>
    /// Id of sub task
    /// </summary>
    public Guid Id { get; set; }

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
    public ToDoTask Parent { get; set; }

    /// <summary>
    /// Constructor for mapping
    /// </summary>
    public Subtask(Guid id, string name, TaskAndSubtaskStatus taskStatus, ToDoTask parent)
    {
        Id = id;
        Name = name;
        TaskStatus = taskStatus;
        Parent = parent;
    }

    /// <summary>
    /// Constructor for creating
    /// </summary>
    public Subtask(string name, TaskAndSubtaskStatus taskStatus, ToDoTask parent)
    {
        Id = Guid.NewGuid();
        Name = name;
        TaskStatus = taskStatus;
        Parent = parent;
    }
}