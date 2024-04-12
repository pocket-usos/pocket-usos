using App.Application.Configuration;
using App.Application.Configuration.Commands;
using App.Application.Users;
using App.Domain.Notifications;
using App.Domain.UserAccess.Authentication;

namespace App.Application.Notifications.ReadNotifications;

public class ReadNotificationsCommandHandler(
    INotificationRepository notificationRepository,
    IAuthenticationSessionRepository authenticationSessionRepository,
    IUserRepository userRepository,
    IExecutionContextAccessor context) : ICommandHandler<ReadNotificationsCommand>
{
    public async Task Handle(ReadNotificationsCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetCurrentAsync();
        var session = await authenticationSessionRepository.GetByIdAsync(new AuthenticationSessionId(context.SessionId));

        var userNotifications = await notificationRepository.GetByInstitutionIdAndUserIdAsync(session.InstitutionId, user.Id);

        var notificationsToMarkAsRead = userNotifications.Where(n => command.NotificationIds.Contains(n.Id.Value)).ToList();

        foreach (var notification in notificationsToMarkAsRead)
        {
            notification.Read();
        }

        await notificationRepository.SaveAsync();
    }
}
