using Mediator;
using TaskService.Features.Shared.Interfaces;
using TaskService.Infrastructure.Persistence.Entities;

namespace TaskService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly TaskDbContext _context;
    private readonly IMediator _mediator;

    public UnitOfWork(TaskDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        var domainEventEntities = _context.ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();
        var events = domainEventEntities.SelectMany(e => e.DomainEvents).ToList();
        domainEventEntities.ForEach(e=>e.DomainEvents.Clear());
        await _context.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(events);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    
    /// <summary>
    /// Method to dispatch domain events
    /// </summary>
    private async Task DispatchDomainEventsAsync(List<INotification> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}