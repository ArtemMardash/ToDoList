using System.ComponentModel;
using MassTransit;
using SharedKernel;
using TaskService.Core.Events;
using TaskService.Features.Shared.Interfaces;

namespace TaskService.Infrastructure;

public class BrokerPublisher : IBrokerPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public BrokerPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishTaskCreatedAsync(TaskCreated task, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish<ITaskCreated>(new
            {
                Id = task.Id,
                UserId = task.UserId,
                Name = task.Name,
                Description = task.Description,
                Deadline = task.Deadline
            }
            , cancellationToken);
        Console.WriteLine($"Published {nameof(ITaskCreated)}");
    }

    public async Task PublishTaskUpdatedAsync(TaskUpdated task, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish<ITaskUpdated>(new
            {
                Id = task.Id,
                UserId = task.UserId,
                Name = task.Name,
                Description = task.Description,
                Deadline = task.Deadline
            }
            , cancellationToken);
        Console.WriteLine($"Published {nameof(ITaskUpdated)}");
    }

    public async Task PublishTaskDeletedAsync(TaskDeleted task, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish<ITaskDeleted>(new
            {
                Id = task.Id,
                UserId = task.UserId,
            }
            , cancellationToken);
        Console.WriteLine($"Published {nameof(ITaskDeleted)}");
    }
}