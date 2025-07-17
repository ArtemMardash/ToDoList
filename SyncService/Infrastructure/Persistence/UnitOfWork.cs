using SyncService.Features.Shared.Interfaces;

namespace SyncService.Infrastructure.Persistence;

public class UnitOfWork: IUnitOfWork
{
    private readonly SyncDbContext _context;

    public UnitOfWork(SyncDbContext context)
    {
        _context = context;
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}