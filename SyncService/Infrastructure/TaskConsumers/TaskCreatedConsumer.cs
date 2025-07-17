using MassTransit;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;

namespace SyncService.Infrastructure.TaskConsumers;

public class TaskCreatedConsumer: IConsumer<ITaskCreated>
{
    private readonly ITaskCreatedUseCase _taskCreatedUseCase;

    public TaskCreatedConsumer(ITaskCreatedUseCase taskCreatedUseCase)
    {
        _taskCreatedUseCase = taskCreatedUseCase;
    }

    public async Task Consume(ConsumeContext<ITaskCreated> context)
    {
        await _taskCreatedUseCase.ExecuteAsync(context.Message, CancellationToken.None);
    }
}