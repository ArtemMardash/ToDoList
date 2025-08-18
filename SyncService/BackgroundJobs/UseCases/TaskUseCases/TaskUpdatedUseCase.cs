using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Services;

namespace SyncService.BackgroundJobs.UseCases.TaskUseCases;

public class TaskUpdatedUseCase : ITaskUpdatedUseCase
{
    private readonly ITaskSyncMappingRepository _taskSyncMappingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly GoogleCalendarService _googleCalendarService;
    private readonly ITgLinksRepository _tgLinksRepository;
    private readonly INotificationRepository _notificationRepository;

    public TaskUpdatedUseCase(ITaskSyncMappingRepository taskSyncMappingRepository, IUnitOfWork unitOfWork,
        GoogleCalendarService googleCalendarService, ITgLinksRepository tgLinksRepository, INotificationRepository notificationRepository)
    {
        _taskSyncMappingRepository = taskSyncMappingRepository;
        _unitOfWork = unitOfWork;
        _googleCalendarService = googleCalendarService;
        _tgLinksRepository = tgLinksRepository;
        _notificationRepository = notificationRepository;
    }

    public async Task ExecuteAsync(ITaskUpdated taskUpdated, CancellationToken cancellationToken)
    {
        var taskToUpdate =
            await _taskSyncMappingRepository.GetTaskSyncMappingAsyncByTaskId(taskUpdated.Id, cancellationToken);
        await _googleCalendarService.InsertOrUpdateEventAsync(taskToUpdate, taskUpdated, cancellationToken);
        await _notificationRepository.CreateNotificationAsync(
            new Notification(taskUpdated.Id, taskUpdated.UserId, NotificationType.Updated, false), 
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        Console.WriteLine("The task should be updated");
        try
        {
            await _tgLinksRepository.GetTgLinkByUserIdAsync(taskUpdated.UserId, cancellationToken);
            
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine("The task should be updated and user is not connected to TG");
        }
    }
}