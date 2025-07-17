namespace SharedKernel;

public interface ITaskDeleted
{
    /// <summary>
    /// Id of task
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id of user that wrote/have this task
    /// </summary>
    public Guid UserId { get; set; }
}