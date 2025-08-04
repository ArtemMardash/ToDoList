using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using SharedKernel;
using SyncService.BackgroundJobs.Dtos;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Services.WebClients;

namespace SyncService.Infrastructure.Services;

public class GoogleCalendarService
{
    private readonly ITaskServiceApi _taskServiceApi;
    private readonly IUserSyncStateRepository _userSyncStateRepository;
    private readonly ITaskSyncMappingRepository _taskSyncMappingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private static readonly string[] _scopes = { CalendarService.Scope.Calendar };

    private const string APP_NAME = "authfortodolist";

    public GoogleCalendarService(ITaskServiceApi taskServiceApi,
        IUserSyncStateRepository userSyncStateRepository,
        ITaskSyncMappingRepository taskSyncMappingRepository,
        IUnitOfWork unitOfWork)
    {
        _taskServiceApi = taskServiceApi;
        _userSyncStateRepository = userSyncStateRepository;
        _taskSyncMappingRepository = taskSyncMappingRepository;
        _unitOfWork = unitOfWork;
    }

    public Task InsertOrUpdateEventAsync(TaskSyncMapping taskSyncMapping, ITaskCreated taskCreted,
        CancellationToken cancellationToken)
    {
        var taskInfo = new TaskInfo
        {
            Id = taskCreted.Id,
            UserId = taskCreted.UserId,
            Name = taskCreted.Name,
            Description = taskCreted.Description,
            End = taskCreted.Deadline,
            Start = taskCreted.Start
        };
        return InsertOrUpdateEventAsync(taskSyncMapping, taskInfo, cancellationToken);
    }

    public Task InsertOrUpdateEventAsync(TaskSyncMapping taskSyncMapping, ITaskUpdated taskUpdated,
        CancellationToken cancellationToken)
    {
        var taskInfo = new TaskInfo
        {
            Id = taskUpdated.Id,
            UserId = taskUpdated.UserId,
            Name = taskUpdated.Name,
            Description = taskUpdated.Description,
            End = taskUpdated.Deadline,
            Start = taskUpdated.Start
        };
        return InsertOrUpdateEventAsync(taskSyncMapping, taskInfo, cancellationToken);
    }

    public async Task DeleteEventAsync(TaskSyncMapping taskSyncMapping, Guid userId,
        CancellationToken cancellationToken)
    {
        var userSyncState = await _userSyncStateRepository.GetUserSyncStateByUserId(userId, cancellationToken);
        var calendarService = await GetCalendarServiceAsync(userSyncState.GoogleAccessToken, cancellationToken);
        await calendarService.Events.Delete("primary", taskSyncMapping.CalendarEventId).ExecuteAsync(cancellationToken);
    }

    private async Task InsertOrUpdateEventAsync(TaskSyncMapping taskSyncMapping, TaskInfo taskInfo,
        CancellationToken cancellationToken)
    {
        var userSyncState = await _userSyncStateRepository.GetUserSyncStateByUserId(taskInfo.UserId, cancellationToken);
        var calendarService = await GetCalendarServiceAsync(userSyncState.GoogleAccessToken, cancellationToken);

        var taskEvent = new Event
        {
            Summary = taskInfo.Name,
            Description = taskInfo.Description,
            End = new EventDateTime
            {
                DateTimeDateTimeOffset = taskInfo.End,
            },
            Start = new EventDateTime
            {
                DateTimeDateTimeOffset = taskInfo.Start
            }
        };
        if (string.IsNullOrEmpty(taskSyncMapping.CalendarEventId))
        {
            var createdEvent =
                await calendarService.Events.Insert(taskEvent, "primary").ExecuteAsync(cancellationToken);
            await _taskSyncMappingRepository.UpdateCalendarEventIdAsync(taskInfo.Id, createdEvent.Id,
                cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await calendarService.Events.Update(taskEvent, "primary", taskSyncMapping.CalendarEventId)
                .ExecuteAsync(cancellationToken);
        }
    }

    private async Task<CalendarService> GetCalendarServiceAsync(string googleAccessToken,
        CancellationToken cancellationToken)
    {
        var initializer = new BaseClientService.Initializer
        {
            HttpClientInitializer = GoogleCredential.FromAccessToken(googleAccessToken),
            ApplicationName = "ToDoList"
        };
        return new CalendarService(initializer);
    }
}