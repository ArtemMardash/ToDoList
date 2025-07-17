using MassTransit;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.BackgroundJobs.UseCases;
using SyncService.Features.Shared.Repositories;

namespace SyncService.Infrastructure.GoogleAccessConsumers;

public class LoginConsumer : IConsumer<IGoogleLogin>
{
    private readonly IGoogleLoginUseCase _googleLoginUseCase;
    private readonly IUserSyncStateRepository _userSyncStateRepository;

    public LoginConsumer(IGoogleLoginUseCase googleLoginUseCase, IUserSyncStateRepository userSyncStateRepository)
    {
        _googleLoginUseCase = googleLoginUseCase;
        _userSyncStateRepository = userSyncStateRepository;
    }

    public async Task Consume(ConsumeContext<IGoogleLogin> context)
    {
        await _googleLoginUseCase.ExecuteAsync(context.Message, context.CancellationToken);
    }
}