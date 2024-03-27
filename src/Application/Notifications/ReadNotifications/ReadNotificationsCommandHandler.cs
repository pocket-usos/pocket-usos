using App.Application.Configuration.Commands;
using App.Application.Users;
using App.Domain.Notifications;

namespace App.Application.Notifications.ReadNotifications;

public class ReadNotificationsCommandHandler(INotificationRepository notificationRepository, IUserRepository userRepository) : ICommandHandler<ReadNotificationsCommand>
{
    public async Task Handle(ReadNotificationsCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetCurrentAsync();

        var userNotifications = await notificationRepository.GetByUserIdAsync(user.Id);

        var notificationsToMarkAsRead = userNotifications.Where(n => command.NotificationIds.Contains(n.Id.Value)).ToList();

        foreach (var notification in notificationsToMarkAsRead)
        {
            notification.Read();
        }

        await notificationRepository.SaveAsync();
    }
}
