using MassTransit;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.BackgroundJobs.UseCases;
using SyncService.Features.Shared.Repositories;

namespace SyncService.Infrastructure.GoogleAccessConsumers;

public class LoginConsumer: IConsumer<IGoogleLogin>
{
    private readonly IGoogleLoginUseCase _googleLoginUseCase;
    private readonly IUserSyncStateRepository _userSyncStateRepository;
    private readonly IGoogleRegisterUseCase _googleRegisterUseCase;

    public LoginConsumer(IGoogleLoginUseCase googleLoginUseCase, IUserSyncStateRepository userSyncStateRepository, IGoogleRegisterUseCase googleRegisterUseCase)
    {
        _googleLoginUseCase = googleLoginUseCase;
        _userSyncStateRepository = userSyncStateRepository;
        _googleRegisterUseCase = googleRegisterUseCase;
    }
    
    public async Task Consume(ConsumeContext<IGoogleLogin> context)
    {
        try
        {
            var user = await _userSyncStateRepository.GetUserSyncStateByUserId(context.Message.UserId,
                context.CancellationToken);
            await _googleLoginUseCase.ExecuteAsync(context.Message, context.CancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            Console.Write("User doesn't exist, register process begins");
        }

    }
    
}