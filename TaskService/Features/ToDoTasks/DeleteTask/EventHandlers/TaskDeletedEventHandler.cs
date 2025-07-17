using Mediator;
using TaskService.Core.Events;
using TaskService.Features.Shared.Interfaces;

namespace TaskService.Features.ToDoTasks.DeleteTask.EventHandlers;

public class TaskDeletedEventHandler: INotificationHandler<TaskDeleted>
{
    private readonly IBrokerPublisher _brokerPublisher;

    public TaskDeletedEventHandler(IBrokerPublisher brokerPublisher)
    {
        _brokerPublisher = brokerPublisher;
    }
    
    public async ValueTask Handle(TaskDeleted notification, CancellationToken cancellationToken)
    {
        await _brokerPublisher.PublishTaskDeletedAsync(notification, cancellationToken);
    }
}