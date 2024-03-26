namespace App.Domain.Notifications;

public class Notification
{
    public NotificationId Id { get; private set; }

    public string UserId { get; private set; }

    public NotificationContent Content { get; private set; }

    public bool WasRead { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public void Read()
    {
        WasRead = true;
    }

    public Notification(string userId, NotificationContent content)
    {
        Id = new NotificationId(Guid.NewGuid());
        UserId = userId;
        Content = content;
        WasRead = false;
        CreatedAt = DateTime.Now;
    }

    private Notification() {}
}
