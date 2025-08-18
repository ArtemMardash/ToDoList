using Microsoft.EntityFrameworkCore;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Mapping;

namespace SyncService.Infrastructure.Persistence.Repositories;

public class TgLinksRepository : ITgLinksRepository
{
    private readonly SyncDbContext _syncDbContext;

    public TgLinksRepository(SyncDbContext syncDbContext)
    {
        _syncDbContext = syncDbContext;
    }

    public Task AddAsync(TgLinks tgLinks, CancellationToken cancellationToken)
    {
        return _syncDbContext.TgLinks.AddAsync(tgLinks.ToDb(), cancellationToken).AsTask();
    }

    public async Task<TgLinks> GetTgLinkByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var tgLinkDb = await _syncDbContext.TgLinks.FirstOrDefaultAsync(tg => tg.UserId == userId, cancellationToken);
        if (tgLinkDb == null)
        {
            throw new InvalidOperationException("There is not tgLink with such Id");
        }

        return tgLinkDb.ToDomain();
    }

    public async Task UpdateAsync(TgLinks tgLinks, CancellationToken cancellationToken)
    {
        var tgLinksDb = await _syncDbContext.TgLinks.FirstOrDefaultAsync(tg => tg.Id == tgLinks.Id, cancellationToken);
        if (tgLinksDb == null)
        {
            throw new InvalidOperationException("There is not tgLink with such Id");
        }
        tgLinksDb.UniqueCode = tgLinks.UniqueCode;
        tgLinksDb.TgUserId = tgLinks.TgUserId;
    }

    public async Task<TgLinks> GetTgLinkByUniqueCodeAsync(int uniqueCode, CancellationToken cancellationToken)
    {
        var tgLinkDb = await _syncDbContext.TgLinks.FirstOrDefaultAsync(tg => tg.UniqueCode == uniqueCode, cancellationToken);
        if (tgLinkDb == null)
        {
            throw new InvalidOperationException("There is not tgLink with such unique code");
        }

        return tgLinkDb.ToDomain();
    }

    public async Task<bool> IsUniqueCodeUniqueAsync(int uniqueCode, CancellationToken cancellationToken)
    {
        return await _syncDbContext.TgLinks.FirstOrDefaultAsync(tg => tg.UniqueCode == uniqueCode,
            cancellationToken) == null;
    }

    public async Task<TgLinks> GetTgLinkByTgId(long tgId, CancellationToken cancellationToken)
    {
        var tgLinkDb = await _syncDbContext.TgLinks.FirstOrDefaultAsync(tg => tg.TgUserId == tgId, cancellationToken);
        if (tgLinkDb == null)
        {
            throw new InvalidOperationException("There is not tgLink with such Tg User Id");
        }

        return tgLinkDb.ToDomain();
    }
}