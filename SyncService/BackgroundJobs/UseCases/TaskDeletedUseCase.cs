using SharedKernel;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases;

public class TaskDeletedUseCase: ITaskDeletedUseCase
{
    private readonly ITaskSyncMappingRepository _taskSyncMappingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskDeletedUseCase(ITaskSyncMappingRepository taskSyncMappingRepository, IUnitOfWork unitOfWork)
    {
        _taskSyncMappingRepository = taskSyncMappingRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task ExecuteAsync(ITaskDeleted taskDeleted, CancellationToken cancellationToken)
    {
        try
        {
            //тут удалить через google caledare
            //ToDO Delete from google calendare
           await _taskSyncMappingRepository.GetTaskSyncMappingAsync(taskDeleted.Id, cancellationToken);
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