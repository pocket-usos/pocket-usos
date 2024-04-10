using App.Application.Contracts;
using App.Application.Notifications;
using App.Application.Notifications.GetOneSignalExternalId;
using OneSignalApi.Model;

namespace App.Infrastructure.Application.Notifications;

internal class PushNotificationSender(IGateway gateway, OneSignalClientFactory oneSignalClientFactory, OneSignalConfiguration configuration) : IPushNotificationSender
{
    public async Task SendAsync(OneSignalPushNotification notification)
    {
        var client = oneSignalClientFactory.GetClient();

        var externalId = await gateway.ExecuteQueryAsync(new GetOneSignalExternalIdQuery(notification.SessionId));

        var oneSignalNotification = new Notification(appId: configuration.AppId)
        {
            ExternalId = notification.Id.ToString(),
            Headings = new StringMap(en: notification.HeadingInEnglish, pl: notification.HeadingInPolish),
            Contents = new StringMap(en: notification.ContentInEnglish, pl: notification.ContentInPolish),
            IncludeExternalUserIds = new List<string> {externalId},
            TargetChannel = Notification.TargetChannelEnum.Push,
            ChannelForExternalUserIds = "push",
        };

        await client.CreateNotificationAsync(oneSignalNotification);
    }
}
