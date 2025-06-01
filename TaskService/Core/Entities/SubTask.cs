
namespace TaskService.Core.Entities;

public class SubTask
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    
    public TaskStatus TaskStatus { get; set; }

    public Task Parent { get; set; }

    /// <summary>
    /// Constructor for mapping
    /// </summary>
    public SubTask(Guid id, string name, TaskStatus taskStatus, Task parent)
    {
        Id = id;
        Name = name;
        TaskStatus = taskStatus;
        Parent = parent;
    }

    /// <summary>
    /// Constructor for creating
    /// </summary>
    public SubTask(string name, TaskStatus taskStatus, Task parent)
    {
        Id = Guid.NewGuid();
        Name = name;
        TaskStatus = taskStatus;
        Parent = parent;
    }
}