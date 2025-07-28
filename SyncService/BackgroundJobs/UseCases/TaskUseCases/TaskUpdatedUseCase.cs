using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Services;

namespace SyncService.BackgroundJobs.UseCases.TaskUseCases;

public class TaskUpdatedUseCase: ITaskUpdatedUseCase
{
    private readonly ITaskSyncMappingRepository _taskSyncMappingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly GoogleCalendarService _googleCalendarService;

    public TaskUpdatedUseCase(ITaskSyncMappingRepository taskSyncMappingRepository, IUnitOfWork unitOfWork, GoogleCalendarService googleCalendarService)
    {
        _taskSyncMappingRepository = taskSyncMappingRepository;
        _unitOfWork = unitOfWork;
        _googleCalendarService = googleCalendarService;
    }
    
    public async Task ExecuteAsync(ITaskUpdated taskUpdated, CancellationToken cancellationToken)
    {
        var taskToUpdate = await _taskSyncMappingRepository.GetTaskSyncMappingAsync(taskUpdated.Id, cancellationToken);
        await _googleCalendarService.InsertOrUpdateEventAsync(taskToUpdate, taskUpdated.UserId, cancellationToken);
        Console.WriteLine("The task should be updated");
    }
}