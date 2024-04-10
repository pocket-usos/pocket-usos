namespace App.Infrastructure.Application.Notifications;

internal class OneSignalConfiguration
{
    public required string BasePath { get; set; }

    public required string AccessToken { get; set; }

    public required string AppId { get; set; }
}
