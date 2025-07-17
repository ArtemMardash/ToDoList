using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;

namespace TaskService.Core.Events;

public class TaskUpdated: INotification
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
}