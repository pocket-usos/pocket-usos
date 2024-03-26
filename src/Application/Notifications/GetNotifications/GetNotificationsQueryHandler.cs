using App.Application.Configuration;
using App.Application.Configuration.Queries;
using App.Application.Users;
using App.Domain.Notifications;

namespace App.Application.Notifications.GetNotifications;

public class GetNotificationsQueryHandler(
    INotificationRepository notificationRepository,
    IUserRepository userRepository,
    IExecutionContextAccessor executionContextAccessor
    ) : IQueryHandler<GetNotificationsQuery, IEnumerable<NotificationDto>>
{
    public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetCurrentAsync();

        var notifications = await notificationRepository.GetByUserIdAsync(user.Id);

        return notifications.Select(n => new NotificationDto
        {
            Id = n.Id.Value,
            Content = executionContextAccessor.Language == "pl" ? n.Content.Pl : n.Content.En,
            WasRead = n.WasRead,
            CreatedAt = n.CreatedAt
        });
    }
}
