using Mediator;
using TaskService.Features.ToDoTasks.GetTaskById.Models;

namespace TaskService.Features.ToDoTasks.GetTasksByCategory.Models;

public class GetTasksByCategoryRequest : IRequest<GetTasksByCategoryResult>
{
    public Guid CategoryId { get; set; }
}

public class GetTasksByCategoryResult
{
    public List<TaskDtoResult> Tasks { get; set; }
}