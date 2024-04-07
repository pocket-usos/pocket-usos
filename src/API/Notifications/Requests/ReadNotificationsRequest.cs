namespace App.API.Notifications.Requests;

public class ReadNotificationsRequest
{
    public Guid[] NotificationIds { get; set; }
}
