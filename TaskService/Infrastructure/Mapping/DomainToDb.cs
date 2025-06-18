using TaskService.Core.Entities;
using TaskService.Infrastructure.Persistence.Entities;

namespace TaskService.Infrastructure.Mapping;

public static class DomainToDb
{
    public static CategoryDb ToDb(this Category category)
    {
        return new CategoryDb
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public static SubTaskDb ToDb(this Subtask subtask)
    {
        return new SubTaskDb
        {
            Id = subtask.Id,
            Name = subtask.Name,
            TaskStatus = (int)subtask.TaskStatus,
            ParentId = subtask.Parent.Id
        };
    }

    public static TaskDb ToDb(this ToDoTask task)
    {
        return new TaskDb
        {
            Id = task.Id,
            UserId = task.UserId,
            Name = task.Name,
            Description = task.Description,
            TaskStatus = (int)task.TaskStatus,
            Category = new CategoryDb
            {
                Id = task.Category.Id,
                Name = task.Category.Name,
                Description = task.Category.Description
            },
            Deadline = task.Deadline,
            SubTasks = task.SubTasks.Select(s => s.ToDb()).ToList()
        };
    }
}