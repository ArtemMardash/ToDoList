using MassTransit;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;

namespace SyncService.Infrastructure.TaskConsumers;

public class TaskDeletedConsumer: IConsumer<ITaskDeleted>
{
    private readonly ITaskDeletedUseCase _taskDeletedUseCase;

    public TaskDeletedConsumer(ITaskDeletedUseCase taskDeletedUseCase)
    {
        _taskDeletedUseCase = taskDeletedUseCase;
    }
    
    public async Task Consume(ConsumeContext<ITaskDeleted> context)
    {
        await _taskDeletedUseCase.ExecuteAsync(context.Message, CancellationToken.None);
    }
}