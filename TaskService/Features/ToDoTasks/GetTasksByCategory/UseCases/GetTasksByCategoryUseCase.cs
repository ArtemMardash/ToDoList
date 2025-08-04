using Mediator;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Features.ToDoTasks.GetTaskById.Models;
using TaskService.Features.ToDoTasks.GetTasksByCategory.Models;

namespace TaskService.Features.ToDoTasks.GetTasksByCategory.UseCases;

public class GetTasksByCategoryUseCase : IRequestHandler<GetTasksByCategoryRequest, GetTasksByCategoryResult>
{
    private readonly ITaskRepository _taskRepository;


    public GetTasksByCategoryUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async ValueTask<GetTasksByCategoryResult> Handle(GetTasksByCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _taskRepository.GetTasksByCategoryAsync(request.CategoryId, cancellationToken);

        return new GetTasksByCategoryResult
        {
            Tasks = result.Select(ToDoTaskToTaskDtoResult).ToList()
        };
    }

    private TaskDtoResult ToDoTaskToTaskDtoResult(ToDoTask task)
    {
        return new TaskDtoResult
        {
            Id = task.Id,
            UserId = task.UserId,
            Name = task.Name,
            Description = task.Description,
            Category = ToCategoryResult(task.Category),
            Start = task.Start,
            Deadline = task.Deadline,
            TaskStatus = task.TaskStatus,
            SubTasks = task.SubTasks.Select(ToSubTaskResult).ToList()
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