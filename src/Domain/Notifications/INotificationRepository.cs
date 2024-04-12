using App.Domain.BuildingBlocks;
using App.Domain.Institutions;

namespace App.Domain.Notifications;

public interface INotificationRepository : IRepository
{
    Task AddAsync(Notification notification);

    Task<Notification> GetByIdAsync(NotificationId id);

    Task<IEnumerable<Notification>> GetByInstitutionIdAndUserIdAsync(InstitutionId institutionId, string userId);
}
