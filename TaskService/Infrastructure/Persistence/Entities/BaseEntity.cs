using System.ComponentModel.DataAnnotations.Schema;
using Mediator;

namespace TaskService.Infrastructure.Persistence.Entities;

public class BaseEntity
{
    /// <summary>
    /// List of notification  
    /// </summary>
    [NotMapped]
    public List<INotification> DomainEvents { get;} = new List<INotification>();
}