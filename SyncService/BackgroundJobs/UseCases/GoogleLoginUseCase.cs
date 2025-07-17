using SharedKernel;
using SyncService.BackgroundJobs.Dtos;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases;

public class GoogleLoginUseCase : IGoogleLoginUseCase
{
    private readonly IUserSyncStateRepository _userSyncStateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleRegisterUseCase _googleRegistredUseCase;

    public GoogleLoginUseCase(IUserSyncStateRepository userSyncStateRepository, IUnitOfWork unitOfWork, IGoogleRegisterUseCase googleRegistredUseCase)
    { 
        _userSyncStateRepository = userSyncStateRepository;
        _unitOfWork = unitOfWork;
        _googleRegistredUseCase = googleRegistredUseCase;
    }

    public async Task ExecuteAsync(IGoogleLogin googleLogin, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userSyncStateRepository.GetUserSyncStateByUserId(googleLogin.UserId, cancellationToken);
            var userToUpdate = new UserSyncState(
                user.Id,
                googleLogin.UserId,
                googleLogin.AccessToken,
                googleLogin.RefreshToken,
                googleLogin.TokenExpiry);
            await _userSyncStateRepository.UpdateUserSyncStateAsync(userToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            var dto = new GoogleRegisterDto
            {
                UserId = googleLogin.UserId,
                GoogleRefreshToken = googleLogin.RefreshToken,
                GoogleAccessToken = googleLogin.AccessToken,
                TokenExpiry = googleLogin.TokenExpiry
            };
            await _googleRegistredUseCase.ExecuteAsync(dto, cancellationToken);
        }
    }
}