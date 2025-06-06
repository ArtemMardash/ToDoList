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
    /// Status of subtask
    /// </summary>
    public int TaskStatus { get; set; }
    
    /// <summary>
    /// Id of parent(original task)
    /// </summary>
    public Guid ParentId { get; set; }
}