using SyncService.BackgroundJobs.Dtos;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases.NotificationUseCases;

public class AddNotificationUseCase: IAddNotificationUseCase
{
    private readonly INotificationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddNotificationUseCase(INotificationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(AddNotificationDto addNotificationDto,
        CancellationToken cancellationToken)
    {
        await _repository
            .CreateNotificationAsync(new Notification(addNotificationDto.TaskId,
                addNotificationDto.UserId,
                addNotificationDto.NotificationType,
                addNotificationDto.IsDelivered), 
                cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}