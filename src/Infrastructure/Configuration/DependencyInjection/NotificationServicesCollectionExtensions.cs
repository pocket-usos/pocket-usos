using App.Application.Notifications;
using App.Infrastructure.Application.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure.Configuration.DependencyInjection;

public static class NotificationServicesCollectionExtensions
{
    public static IServiceCollection AddNotifications(this IServiceCollection services, IConfiguration configuration)
    {
        var oneSignalConfiguration = configuration.GetSection("OneSignal").Get<OneSignalConfiguration>()!;

        services.AddSingleton(oneSignalConfiguration);
        services.AddScoped<OneSignalClientFactory>(_ => new OneSignalClientFactory(oneSignalConfiguration));
        services.AddScoped<IPushNotificationSender, PushNotificationSender>();

        return services;
    }
}
