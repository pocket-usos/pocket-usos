namespace App.Application.Notifications;

public class NotificationDto
{
    public required Guid Id { get; init; }

    public required string Type { get; init; }

    public required string Content { get; init; }

    public required bool WasRead { get; init; }

    public required DateTime CreatedAt { get; init; }
}
