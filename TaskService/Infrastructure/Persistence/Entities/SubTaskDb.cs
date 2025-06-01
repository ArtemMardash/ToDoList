namespace TaskService.Infrastructure.Persistence.Entities;

public class SubTaskDb
{
    /// <summary>
    /// Id of subtask
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Name of subtask
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Description of subtask
    /// </summary>
    public string Description { get; set; }
    
    public DateTime Deadline { get; set; }
    
    public virtual TaskDb Parent { get; set; }
    
    public Guid ParentId { get; set; }
}