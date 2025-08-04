
namespace SharedKernel;

public interface ITaskCreated
{
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
    /// Deadline of the task
    /// </summary>
    public DateTime Deadline { get; set; }
    
    /// <summary>
    /// Start of the task
    /// </summary>
    public DateTime Start { get; set; }
}