using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases;

public class TaskCreatedUseCase: ITaskCreatedUseCase
{
    private readonly ITaskSyncMappingRepository _taskSyncMappingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskCreatedUseCase(ITaskSyncMappingRepository taskSyncMappingRepository, IUnitOfWork unitOfWork)
    {
        _taskSyncMappingRepository = taskSyncMappingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(ITaskCreated taskCreated, CancellationToken cancellationToken)
    {
        try
        {
            var taskSyncMapping = await _taskSyncMappingRepository.GetTaskSyncMappingAsync(taskCreated.Id, cancellationToken);
            Console.WriteLine("Task with such Id already exists, event ignored");
        }
        catch (InvalidOperationException ex)
        {
            //ToDo Add to google Calendare and create new TaskSyncMapping
            Console.WriteLine("Created Task Sync Mapping");
            var taskSyncMapping = new TaskSyncMapping(taskCreated.Id, taskCreated.Description);
            await _taskSyncMappingRepository.AddTaskSyncMappingAsync(taskSyncMapping, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}