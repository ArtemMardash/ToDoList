using SyncService.BackgroundJobs.Dtos;
using SyncService.Core.Entities;

namespace SyncService.BackgroundJobs.Interfaces;

public interface IUpdateNotificationUseCase
{
    public Task ExecuteAsync(UpdateNotificationDto updateNotificationDto, CancellationToken cancellationToken);
}