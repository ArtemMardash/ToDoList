using Mediator;
using TaskService.Core.Events;
using TaskService.Features.Shared.Interfaces;

namespace TaskService.Features.ToDoTasks.AddTask.EventHandlers;

public class TaskCreatedEventHandler: INotificationHandler<TaskCreated>
{
    private readonly IBrokerPublisher _brokerPublisher;

    public TaskCreatedEventHandler(IBrokerPublisher brokerPublisher)
    {
        _brokerPublisher = brokerPublisher;
    }
    
    public async ValueTask Handle(TaskCreated notification, CancellationToken cancellationToken)
    {
       await _brokerPublisher.PublishTaskCreatedAsync(notification, cancellationToken);
    }
}