using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases;

public class TaskUpdatedUseCase: ITaskUpdatedUseCase
{
    private readonly ITaskSyncMappingRepository _taskSyncMappingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskUpdatedUseCase(ITaskSyncMappingRepository taskSyncMappingRepository, IUnitOfWork unitOfWork)
    {
        _taskSyncMappingRepository = taskSyncMappingRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task ExecuteAsync(ITaskUpdated taskUpdated, CancellationToken cancellationToken)
    {
        var taskToUpdate = await _taskSyncMappingRepository.GetTaskSyncMappingAsync(taskUpdated.Id, cancellationToken);
        //ToDo Http client for task service pull new data from task Service and push to google
        Console.WriteLine("The task should be updated");
    }
}