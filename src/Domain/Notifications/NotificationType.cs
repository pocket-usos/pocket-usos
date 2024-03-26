namespace App.Domain.Notifications;

public record NotificationType(string Value)
{
    public static NotificationType Grades => new("Grades");

    public static NotificationType Schedule => new("Schedule");
}
