namespace App.Application.Notifications;

public class OneSignalPushNotification
{
    public required Guid Id { get; set; }

    public required Guid SessionId { get; set; }

    public required string HeadingInEnglish { get; set; }

    public required string HeadingInPolish { get; set; }

    public required string ContentInEnglish { get; set; }

    public required string ContentInPolish { get; set; }
}
