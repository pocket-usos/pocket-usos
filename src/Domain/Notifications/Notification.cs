using App.Domain.Institutions;

namespace App.Domain.Notifications;

public class Notification
{
    public NotificationId Id { get; private set; }

    public string UserId { get; private set; }

    public InstitutionId InstitutionId { get; private set; }

    public NotificationType Type { get; private set; }

    public NotificationContent Content { get; private set; }

    public bool WasRead { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public void Read()
    {
        WasRead = true;
    }

    public Notification(string userId, InstitutionId institutionId, NotificationType type, NotificationContent content)
    {
        Id = new NotificationId(Guid.NewGuid());
        UserId = userId;
        InstitutionId = institutionId;
        Type = type;
        Content = content;
        WasRead = false;
        CreatedAt = DateTime.UtcNow;
    }

    private Notification() {}
}
