using TaskService.Core.Interfaces;

namespace TaskService.Core.Entities;

public class Task: ITask
{
    /// <summary>
    /// Id of task
    /// </summary>
    public Guid Id { get; set; }
    
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
    public Category Category { get; set; }

    /// <summary>
    /// Deadline of the task
    /// </summary>
    public DateTime Deadline { get; set; }
    
    /// <summary>
    /// Subtasks for one main task
    /// </summary>
    public List<ITask> SubTasks { get; set; }
    
    /// <summary>
    /// Constructor for creation
    /// </summary>
    public Task(Guid id, string name, string description, Category category, DateTime deadline, List<ITask>? subTasks)
    {
        Id = id;
        Name = name;
        Description = description;
        Category = category;
        Deadline = deadline;
        SubTasks = subTasks ?? new List<ITask>();
    }

    /// <summary>
    /// Constructor for mapping
    /// </summary>
    public Task(Guid id, string name, string description, DateTime deadline, List<ITask>? subTasks)
    {
        Id = id;
        Name = name;
        Description = description;
        Deadline = deadline;
        SubTasks = subTasks ?? new List<ITask>();
    }


}