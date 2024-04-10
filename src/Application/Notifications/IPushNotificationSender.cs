namespace App.Application.Notifications;

public interface IPushNotificationSender
{
    public Task SendAsync(OneSignalPushNotification notification);
}
