using App.Application.Contracts;

namespace App.Application.Notifications.GetNotifications;

public class GetNotificationsQuery : QueryBase<IEnumerable<NotificationDto>>;
