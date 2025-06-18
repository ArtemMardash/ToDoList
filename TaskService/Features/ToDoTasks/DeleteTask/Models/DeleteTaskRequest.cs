using Mediator;

namespace TaskService.Features.ToDoTasks.DeleteTask.Models;

public class DeleteTaskRequest : IRequest
{
    public Guid Id { get; set; }
}