using App.Application.Configuration;
using App.Application.Configuration.Queries;
using App.Application.Users;
using App.Domain.Notifications;
using App.Domain.UserAccess.Authentication;

namespace App.Application.Notifications.GetNotifications;

public class GetNotificationsQueryHandler(
    INotificationRepository notificationRepository,
    IUserRepository userRepository,
    IAuthenticationSessionRepository authenticationSessionRepository,
    IExecutionContextAccessor context
    ) : IQueryHandler<GetNotificationsQuery, IEnumerable<NotificationDto>>
{
    public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetCurrentAsync();
        var session = await authenticationSessionRepository.GetByIdAsync(new AuthenticationSessionId(context.SessionId));

        var notifications = await notificationRepository.GetByInstitutionIdAndUserIdAsync(session.InstitutionId, user.Id);

        return notifications.Select(n => new NotificationDto
        {
            Id = n.Id.Value,
            Type = n.Type.Value,
            Content = context.Language == "pl" ? n.Content.Pl : n.Content.En,
            WasRead = n.WasRead,
            CreatedAt = n.CreatedAt
        }).OrderByDescending(n => n.CreatedAt);
    }
}
