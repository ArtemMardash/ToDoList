using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
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

    public async Task InsertOrUpdateEventAsync(TaskSyncMapping taskSyncMapping, Guid userId,
        CancellationToken cancellationToken)
    {
        var userSyncState = await _userSyncStateRepository.GetUserSyncStateByUserId(userId, cancellationToken);
        var task = await _taskServiceApi.GetTaskInfo(taskSyncMapping.TaskId);
        var calendarService = await GetCalendarServiceAsync(userSyncState.GoogleId, cancellationToken);

        var taskEvent = new Event
        {
            Summary = task.Name,
            Description = task.Description,
            End = new EventDateTime
            {
                DateTimeDateTimeOffset = task.Deadline,
            }
        };
        if (string.IsNullOrEmpty(taskSyncMapping.CalendarEventId))
        {
            var createdEvent =
                await calendarService.Events.Insert(taskEvent, "primary").ExecuteAsync(cancellationToken);
            await _taskSyncMappingRepository.UpdateCalendarEventIdAsync(task.Id, createdEvent.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await calendarService.Events.Update(taskEvent, "primary", taskSyncMapping.CalendarEventId)
                .ExecuteAsync(cancellationToken);
        }
    }

    public async Task DeleteEventAsync(TaskSyncMapping taskSyncMapping, Guid userId ,CancellationToken cancellationToken)
    {
        var userSyncState = await _userSyncStateRepository.GetUserSyncStateByUserId(userId, cancellationToken);
        var calendarService = await GetCalendarServiceAsync(userSyncState.GoogleId, cancellationToken);
        await calendarService.Events.Delete("primary", taskSyncMapping.CalendarEventId).ExecuteAsync(cancellationToken);
    }

    private async Task<CalendarService> GetCalendarServiceAsync(string googleUserId,
        CancellationToken cancellationToken)
    {
        UserCredential credential;
        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                _scopes,
                googleUserId,
                CancellationToken.None
            );
        }

        return new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = APP_NAME
        });
    }
}