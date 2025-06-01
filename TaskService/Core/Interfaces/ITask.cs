using TaskService.Core.Entities;

namespace TaskService.Core.Interfaces;

public interface ITask
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
}