using SyncService.Core.Entities;

namespace SyncService.Features.Shared.Repositories;

public interface ITgLinksRepository
{
    public Task AddAsync(TgLinks tgLinks, CancellationToken cancellationToken);

    public Task<TgLinks> GetTgLinkByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    public Task UpdateAsync(TgLinks tgLinks, CancellationToken cancellationToken);

    public Task<TgLinks> GetTgLinkByUniqueCodeAsync(int uniqueCode, CancellationToken cancellationToken);

    public Task<bool> IsUniqueCodeUniqueAsync(int uniqueCode, CancellationToken cancellationToken);

    public Task<TgLinks> GetTgLinkByTgId(long tgId, CancellationToken cancellationToken);
}