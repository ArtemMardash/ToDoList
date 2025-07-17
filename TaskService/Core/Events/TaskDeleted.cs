using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;

namespace TaskService.Core.Events;

public class TaskDeleted: INotification
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