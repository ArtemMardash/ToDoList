
namespace TaskService.Core.Entities;

public class Task
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
    /// Deadline of the task
    /// </summary>
    public DateTime Deadline { get; set; }

    /// <summary>
    /// Status of the task
    /// </summary>
    public TaskStatus TaskStatus { get; set; }
    
    /// <summary>
    /// Category of the task
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Subtasks for one main task
    /// </summary>
    public List<SubTask> SubTasks { get; set; }
    
    /// <summary>
    /// Constructor for creation
    /// </summary>
    public Task(Guid id, string name, string description, Category category, DateTime deadline, TaskStatus taskStatus ,List<SubTask>? subTasks)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Category = category;
        Deadline = deadline;
        TaskStatus = taskStatus;
        SubTasks = subTasks ?? new List<SubTask>();
    }

    /// <summary>
    /// Constructor for mapping
    /// </summary>
    public Task(Guid id, string name, string description, DateTime deadline, TaskStatus taskStatus, List<SubTask>? subTasks)
    {
        Id = id;
        Name = name;
        Description = description;
        Deadline = deadline;
        TaskStatus = taskStatus;
        SubTasks = subTasks ?? new List<SubTask>();
    }


}