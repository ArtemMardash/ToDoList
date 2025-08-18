using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases.GoogleUseCases;

public class GoogleRegistredUseCase : IGoogleRegisterUseCase
{
    private readonly IUserSyncStateRepository _userSyncStateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITgLinksRepository _tgLinksRepository;
    private readonly IGenerateUniqueCodeUseCase _generateUniqueCodeUseCase;

    public GoogleRegistredUseCase(IUserSyncStateRepository userSyncStateRepository, IUnitOfWork unitOfWork,
        ITgLinksRepository tgLinksRepository, IGenerateUniqueCodeUseCase generateUniqueCodeUseCase)
    {
        _userSyncStateRepository = userSyncStateRepository;
        _unitOfWork = unitOfWork;
        _tgLinksRepository = tgLinksRepository;
        _generateUniqueCodeUseCase = generateUniqueCodeUseCase;
    }

    public async Task ExecuteAsync(IGoogleRegistered googleRegistered, CancellationToken cancellationToken)
    {
        try
        {
            var _ = await _userSyncStateRepository.GetUserSyncStateByUserId(googleRegistered.UserId,
                cancellationToken);
            Console.WriteLine("User with such Id already exists, IGNORE");
        }
        catch (InvalidOperationException ex)
        {
            await _userSyncStateRepository.AddUserSyncStateAsync(
                new UserSyncState(googleRegistered.UserId,
                    googleRegistered.GoogleAccessToken,
                    googleRegistered.GoogleRefreshToken,
                    googleRegistered.GoogleId,
                    googleRegistered.TokenExpiry),
                cancellationToken);
            await _generateUniqueCodeUseCase.ExecuteAsync(googleRegistered.UserId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}