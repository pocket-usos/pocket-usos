using App.Domain.Institutions;
using App.Domain.Notifications;
using App.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Domain.Notifications;

public class NotificationRepository(Context context) : INotificationRepository
{
    public async Task AddAsync(Notification notification)
    {
        await context.AddAsync(notification);
    }

    public async Task<Notification> GetByIdAsync(NotificationId id)
    {
        var notification = await context.Notifications.SingleOrDefaultAsync(n => n.Id == id);

        if (notification is null)
        {
            throw new NotFoundRepositoryException<Notification>(id.Value);
        }

        return notification;
    }

    public async Task<IEnumerable<Notification>> GetByInstitutionIdAndUserIdAsync(InstitutionId institutionId, string userId)
    {
        return await context.Notifications.Where(n => n.UserId == userId && n.InstitutionId.Equals(institutionId)).ToListAsync();
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
