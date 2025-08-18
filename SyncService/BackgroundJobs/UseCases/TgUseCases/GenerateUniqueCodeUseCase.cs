using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases.TgUseCases;

public class GenerateUniqueCodeUseCase : IGenerateUniqueCodeUseCase
{
    private readonly ITgLinksRepository _tgLinksRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateUniqueCodeUseCase(ITgLinksRepository tgLinksRepository, IUnitOfWork unitOfWork)
    {
        _tgLinksRepository = tgLinksRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid userId, CancellationToken cancellationToken)
    {
        TgLinks tgLink;
        try
        {
            tgLink = await _tgLinksRepository.GetTgLinkByUserIdAsync(userId, cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            tgLink = new TgLinks(userId, null, null);
            await _tgLinksRepository.AddAsync(tgLink, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (tgLink.UniqueCode == null)
        {
            int code = new Random().Next(000000, 999999);
            while (!await _tgLinksRepository.IsUniqueCodeUniqueAsync(code, cancellationToken))
            {
                code = new Random().Next(000000, 999999);
            }

            tgLink.UniqueCode = code;
            await _tgLinksRepository.UpdateAsync(tgLink, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            Console.WriteLine("This user already has a unique code");
        }
    }
}