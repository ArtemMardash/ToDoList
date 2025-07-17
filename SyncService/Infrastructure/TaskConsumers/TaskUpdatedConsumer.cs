using MassTransit;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;

namespace SyncService.Infrastructure.TaskConsumers;

public class TaskUpdatedConsumer: IConsumer<ITaskUpdated>
{
    private readonly ITaskUpdatedUseCase _taskUpdatedUseCase;

    public TaskUpdatedConsumer(ITaskUpdatedUseCase taskUpdatedUseCase)
    {
        _taskUpdatedUseCase = taskUpdatedUseCase;
    }
    
    public async Task Consume(ConsumeContext<ITaskUpdated> context)
    {
        await _taskUpdatedUseCase.ExecuteAsync(context.Message, CancellationToken.None);
    }
}