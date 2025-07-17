namespace TaskService.Core.Entities;

public class Category
{
    private const int NAME_MAX_LENGTH = 30;
    
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
        SetName(name);
        SetDescription(description);
    }

    /// <summary>
    /// Constructor for creating
    /// </summary>
    public Category(string name, string description)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetDescription(description);
    }

    private void SetName(string input)
    {
        if (input.Length > NAME_MAX_LENGTH || string.IsNullOrEmpty(input))
        {
            throw new InvalidOperationException("Invalid name of the category");
        }
        else
        {
            Name = input;
        }
    }

    private void SetDescription(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new InvalidOperationException("Invalid description of the category");
        }
        else
        {
            Description = input;
        }
    }
}