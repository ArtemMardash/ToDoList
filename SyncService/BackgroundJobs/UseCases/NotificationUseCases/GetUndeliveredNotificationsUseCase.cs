using SyncService.BackgroundJobs.Interfaces;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;

namespace SyncService.BackgroundJobs.UseCases.NotificationUseCases;

public class GetUndeliveredNotificationsUseCase: IGetUndeliveredNotificationsUseCase
{
    private readonly INotificationRepository _notificationRepository;

    public GetUndeliveredNotificationsUseCase(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    
    public Task<List<Notification>> ExecuteAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _notificationRepository.GetNotDeliveredNotificationsByUserIdAsync(userId, cancellationToken);
    }
}