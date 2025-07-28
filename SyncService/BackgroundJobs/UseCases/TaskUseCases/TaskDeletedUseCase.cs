using Google.Apis.Calendar.v3;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Services;

namespace SyncService.BackgroundJobs.UseCases.TaskUseCases;

public class TaskDeletedUseCase: ITaskDeletedUseCase
{
    private readonly ITaskSyncMappingRepository _taskSyncMappingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly GoogleCalendarService _googleCalendarService;

    public TaskDeletedUseCase(ITaskSyncMappingRepository taskSyncMappingRepository, IUnitOfWork unitOfWork, GoogleCalendarService googleCalendarService)
    {
        _taskSyncMappingRepository = taskSyncMappingRepository;
        _unitOfWork = unitOfWork;
        _googleCalendarService = googleCalendarService;
    }
    
    public async Task ExecuteAsync(ITaskDeleted taskDeleted, CancellationToken cancellationToken)
    {
        try
        {
           var taskSyncMapping = await _taskSyncMappingRepository.GetTaskSyncMappingAsync(taskDeleted.Id, cancellationToken);
           await _googleCalendarService.DeleteEventAsync(taskSyncMapping, taskDeleted.UserId, cancellationToken);
           await _taskSyncMappingRepository.DeleteTaskSyncMappingAsync(taskDeleted.Id, cancellationToken);
           await _unitOfWork.SaveChangesAsync(cancellationToken);
           Console.WriteLine("The task was deleted");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("TaskSyncMapping with such Id doesn't exist");
        }
    }
}