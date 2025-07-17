using TaskService.Core.Events;

namespace TaskService.Features.Shared.Interfaces;

public interface IBrokerPublisher
{
    /// <summary>
    /// Method to publish  when task created
    /// </summary>
    Task PublishTaskCreatedAsync(TaskCreated task, CancellationToken cancellationToken);

    /// <summary>
    /// Method to publish when task updated
    /// </summary>
    Task PublishTaskUpdatedAsync(TaskUpdated taskUpdated, CancellationToken cancellationToken);

    /// <summary>
    /// Method to publish deleted task
    /// </summary>
    Task PublishTaskDeletedAsync(TaskDeleted taskDeleted, CancellationToken cancellationToken);
}