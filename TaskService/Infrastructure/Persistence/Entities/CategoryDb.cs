namespace TaskService.Infrastructure.Persistence.Entities;

public class CategoryDb
{
    /// <summary>
    /// Id of category
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Name of category
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Description of category
    /// </summary>
    public string Description { get; set; }
}