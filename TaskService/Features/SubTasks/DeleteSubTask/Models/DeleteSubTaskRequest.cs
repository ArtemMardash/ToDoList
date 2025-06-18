using Mediator;

namespace TaskService.Features.SubTasks.DeleteSubTask.Models;

public class DeleteSubTaskRequest : IRequest
{
    public Guid Id { get; set; }
}