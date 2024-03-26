using App.Domain.BuildingBlocks;

namespace App.Domain.Notifications;

public interface INotificationRepository : IRepository
{
    Task AddAsync(Notification notification);

    Task<Notification> GetByIdAsync(NotificationId id);

    Task<IEnumerable<Notification>> GetByUserIdAsync(string userId);
}
