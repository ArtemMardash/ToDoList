using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Infrastructure.Persistence.Entities;

namespace TaskService.Infrastructure.Mapping;

public static class DbToDomain
{
    public static Category ToDomain(this CategoryDb categoryDb)
    {
        return new Category(categoryDb.Id, categoryDb.Name, categoryDb.Description);
    }

    public static ToDoTask ToDomain(this TaskDb taskDb)
    {
        var taskStatus = (TaskAndSubtaskStatus)taskDb.TaskStatus;
        var task = new ToDoTask(
            taskDb.Id,
            taskDb.UserId,
            taskDb.Name,
            taskDb.Description,
            taskDb.Category.ToDomain(),
            taskDb.Start,
            taskDb.Deadline,
            taskStatus,
            new List<Subtask>());

        var subTasks = taskDb.SubTasks.Select(s => s.ToDomain(task));
        task.SubTasks = subTasks.ToList();

        return task;
    }

    public static Subtask ToDomain(this SubTaskDb subTaskDb, ToDoTask task)
    {
        return new Subtask(
            subTaskDb.Id,
            subTaskDb.Name,
            (TaskAndSubtaskStatus)subTaskDb.TaskStatus,
            task);
    }
}