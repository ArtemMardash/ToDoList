namespace TaskService.Core.Entities;

public class Category
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

    /// <summary>
    /// Constructor for mapping
    /// </summary>
    public Category(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Constructor for creating
    /// </summary>
    public Category(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }
}