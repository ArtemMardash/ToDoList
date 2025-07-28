using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases.GoogleUseCases;

public class GoogleRegistredUseCase: IGoogleRegisterUseCase
{
    private readonly IUserSyncStateRepository _userSyncStateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GoogleRegistredUseCase(IUserSyncStateRepository userSyncStateRepository, IUnitOfWork unitOfWork)
    {
        _userSyncStateRepository = userSyncStateRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task ExecuteAsync(IGoogleRegistered googleRegistered, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userSyncStateRepository.GetUserSyncStateByUserId(googleRegistered.UserId,
                cancellationToken);
            if (user != null)
            {
                Console.WriteLine("User with such Id already exists, IGNORE");
            }
        }
        catch (InvalidOperationException ex)
        {
            var userSyncState = await _userSyncStateRepository.AddUserSyncStateAsync(
                new UserSyncState(googleRegistered.UserId, googleRegistered.GoogleAccessToken,
                    googleRegistered.GoogleRefreshToken,googleRegistered.GoogleId, googleRegistered.TokenExpiry), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}