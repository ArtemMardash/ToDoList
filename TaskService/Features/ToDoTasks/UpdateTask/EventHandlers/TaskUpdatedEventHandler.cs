using Mediator;
using TaskService.Core.Events;
using TaskService.Features.Shared.Interfaces;

namespace TaskService.Features.ToDoTasks.UpdateTask.TaskUpdatedEventHandler;

public class TaskUpdatedEventHandler: INotificationHandler<TaskUpdated>
{
    private readonly IBrokerPublisher _brokerPublisher;

    public TaskUpdatedEventHandler(IBrokerPublisher brokerPublisher)
    {
        _brokerPublisher = brokerPublisher;
    }
    
    public async ValueTask Handle(TaskUpdated notification, CancellationToken cancellationToken)
    {
        await _brokerPublisher.PublishTaskUpdatedAsync(notification, cancellationToken);
    }
}