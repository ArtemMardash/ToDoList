using Refit;

namespace SyncService.Infrastructure.Services.WebClients;

public interface ITaskServiceApi
{
    [Get("/api/task/get/{taskId}")]
    //[Headers("Authorization: Bearer")]
    Task<TaskDto> GetTaskInfo(Guid taskId);
}

public class TaskDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime Deadline { get; set; }
}