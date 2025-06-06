
using TaskService.Core.Enums;

namespace TaskService.Core.Entities;

public class SubTask
{
    /// <summary>
    /// Id of sub task
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of sub task
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Status of subtask
    /// </summary>
    public TaskAndSubTaskStatus TaskStatus { get; set; }

    /// <summary>
    /// Original task 
    /// </summary>
    public ToDoTask Parent { get; set; }

    /// <summary>
    /// Constructor for mapping
    /// </summary>
    public SubTask(Guid id, string name, TaskAndSubTaskStatus taskStatus, ToDoTask parent)
    {
        Id = id;
        Name = name;
        TaskStatus = taskStatus;
        Parent = parent;
    }

    /// <summary>
    /// Constructor for creating
    /// </summary>
    public SubTask(string name, TaskAndSubTaskStatus taskStatus, ToDoTask parent)
    {
        Id = Guid.NewGuid();
        Name = name;
        TaskStatus = taskStatus;
        Parent = parent;
    }
}