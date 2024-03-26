namespace App.Application.Notifications;

public class NotificationDto
{
    public required Guid Id { get; set; }

    public required string Content { get; set; }

    public required bool WasRead { get; set; }

    public required DateTime CreatedAt { get; set; }
}
