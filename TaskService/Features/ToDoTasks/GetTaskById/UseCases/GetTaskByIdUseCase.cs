using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features.Categories.GetCategoryById.Models;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.ToDoTasks.GetTaskById.Models;

namespace TaskService.Features.ToDoTasks.GetTaskById.UseCases;

public class GetTaskByIdUseCase : IRequestHandler<GetTaskByIdRequest, TaskDtoResult>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async ValueTask<TaskDtoResult> Handle(GetTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _taskRepository.GetTaskByIdAsync(request.Id, cancellationToken);
        return new TaskDtoResult
        {
            Id = result.Id,
            UserId = result.UserId,
            Name = result.Name,
            Description = result.Description,
            Category = ToCategoryResult(result.Category),
            Start = result.Start,
            Deadline = result.Deadline,
            TaskStatus = result.TaskStatus,
            SubTasks = result.SubTasks.Select(ToSubTaskResult).ToList()
        };

        TaskDtoResult.CategoryResult ToCategoryResult(Category category)
        {
            return new TaskDtoResult.CategoryResult
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        TaskDtoResult.SubTaskResult ToSubTaskResult(Subtask subTask)
        {
            return new TaskDtoResult.SubTaskResult
            {
                Id = subTask.Id,
                Name = subTask.Name,
                TaskStatus = (int)subTask.TaskStatus
            };
        }
    }
}