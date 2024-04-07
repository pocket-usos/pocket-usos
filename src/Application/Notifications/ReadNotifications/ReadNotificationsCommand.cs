using App.Application.Contracts;

namespace App.Application.Notifications.ReadNotifications;

public class ReadNotificationsCommand(Guid[] notificationIds) : CommandBase
{
    public Guid[] NotificationIds { get; } = notificationIds;
}
