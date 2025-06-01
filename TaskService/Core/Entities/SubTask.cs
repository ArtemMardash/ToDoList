using TaskService.Core.Interfaces;

namespace TaskService.Core.Entities;

public class SubTask: ITask
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public Category Category { get; set; }

    public DateTime Deadline { get; set; }
    
    public ITask Parent { get; set; }

    public SubTask(Guid id, string name, string description, DateTime deadline, ITask parent)
    {
        Id = id;
        Name = name;
        Description = description;
        Deadline = deadline;
        Parent = parent;
    }

    public SubTask( string name, string description, DateTime deadline, ITask parent)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Deadline = deadline;
        Parent = parent;
    }
}