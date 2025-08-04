using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Services;

namespace SyncService.BackgroundJobs.UseCases.TaskUseCases;

public class TaskCreatedUseCase: ITaskCreatedUseCase
{
    private readonly ITaskSyncMappingRepository _taskSyncMappingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly GoogleCalendarService _googleCalendarService;

    public TaskCreatedUseCase(ITaskSyncMappingRepository taskSyncMappingRepository, IUnitOfWork unitOfWork, GoogleCalendarService googleCalendarService)
    {
        _taskSyncMappingRepository = taskSyncMappingRepository;
        _unitOfWork = unitOfWork;
        _googleCalendarService = googleCalendarService;
    }

    public async Task ExecuteAsync(ITaskCreated taskCreated, CancellationToken cancellationToken)
    {
        try
        {
            var taskSyncMapping = await _taskSyncMappingRepository.GetTaskSyncMappingAsyncByTaskId(taskCreated.Id, cancellationToken);
            Console.WriteLine("Task with such Id already exists, event ignored");
        }
        catch (InvalidOperationException ex)
        {
            var taskSyncMapping = new TaskSyncMapping(taskCreated.Id, "");
            await _taskSyncMappingRepository.AddTaskSyncMappingAsync(taskSyncMapping, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _googleCalendarService.InsertOrUpdateEventAsync(taskSyncMapping, taskCreated, cancellationToken);
            Console.WriteLine("Created Task Sync Mapping");
        }
    }
}