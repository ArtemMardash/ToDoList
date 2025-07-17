using TaskService.Core.Enums;

namespace TaskService.Core.Entities;

public class Subtask
{
    private const int NAME_MAX_LENGTH = 30;
    
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
        SetName(name);
        TaskStatus = taskStatus;
        Parent = parent;
    }
    
    private void SetName(string input)
    {
        if (input.Length > NAME_MAX_LENGTH || string.IsNullOrEmpty(input))
        {
            throw new InvalidOperationException("Invalid name of the category");
        }
        else
        {
            Name = input;
        }
    }
}