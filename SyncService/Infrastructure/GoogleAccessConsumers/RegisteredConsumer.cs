using MassTransit;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;

namespace SyncService.Infrastructure.GoogleAccessConsumers;

public class RegisteredConsumer: IConsumer<IGoogleRegistered>
{
    private readonly IGoogleRegisterUseCase _googleRegisterUseCase;

    public RegisteredConsumer(IGoogleRegisterUseCase googleRegisterUseCase)
    {
        _googleRegisterUseCase = googleRegisterUseCase;
    }
    
    public async Task Consume(ConsumeContext<IGoogleRegistered> context)
    {
        await _googleRegisterUseCase.ExecuteAsync(context.Message, context.CancellationToken);
    }
}