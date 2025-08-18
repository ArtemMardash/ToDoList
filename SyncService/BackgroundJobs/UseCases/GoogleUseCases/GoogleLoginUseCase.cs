using SharedKernel;
using SyncService.BackgroundJobs.Dtos;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases.GoogleUseCases;

public class GoogleLoginUseCase : IGoogleLoginUseCase
{
    private readonly IUserSyncStateRepository _userSyncStateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleRegisterUseCase _googleRegistredUseCase;
    private readonly IGenerateUniqueCodeUseCase _generateUniqueCodeUseCase;
    private readonly ITgLinksRepository _tgLinksRepository;

    public GoogleLoginUseCase(IUserSyncStateRepository userSyncStateRepository,
        IUnitOfWork unitOfWork,
        IGoogleRegisterUseCase googleRegistredUseCase,
        IGenerateUniqueCodeUseCase generateUniqueCodeUseCase,
        ITgLinksRepository tgLinksRepository)
    {
        _userSyncStateRepository = userSyncStateRepository;
        _unitOfWork = unitOfWork;
        _googleRegistredUseCase = googleRegistredUseCase;
        _generateUniqueCodeUseCase = generateUniqueCodeUseCase;
        _tgLinksRepository = tgLinksRepository;
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
                googleLogin.GoogleId,
                googleLogin.TokenExpiry);
            await _userSyncStateRepository.UpdateUserSyncStateAsync(userToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            var dto = new GoogleRegisterDto
            {
                UserId = googleLogin.UserId,
                GoogleId = googleLogin.GoogleId,
                GoogleRefreshToken = googleLogin.RefreshToken,
                GoogleAccessToken = googleLogin.AccessToken,
                TokenExpiry = googleLogin.TokenExpiry
            };
            await _googleRegistredUseCase.ExecuteAsync(dto, cancellationToken);
        }

        await _generateUniqueCodeUseCase.ExecuteAsync(googleLogin.UserId, cancellationToken);
    }
}