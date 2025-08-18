using SyncService.BackgroundJobs.Dtos;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases.NotificationUseCases;

public class UpdateNotificationUseCase: IUpdateNotificationUseCase
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNotificationUseCase(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task ExecuteAsync(UpdateNotificationDto updateNotificationDto, CancellationToken cancellationToken)
    {
        var notification = new Notification(
            updateNotificationDto.Id, 
            updateNotificationDto.TaskId,
            updateNotificationDto.UserId, 
            updateNotificationDto.NotificationType, 
            updateNotificationDto.IsDelivered);

        await _notificationRepository.DeliverNotificationAsync(notification, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}