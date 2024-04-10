using OneSignalApi.Api;

namespace App.Infrastructure.Application.Notifications;

internal class OneSignalClientFactory(OneSignalConfiguration configuration)
{
    private DefaultApi? _client;

    public DefaultApi GetClient()
    {
        if (_client is null)
        {
            _client = CreateClient();
        }

        return _client;
    }

    private DefaultApi CreateClient()
    {
        var clientConfiguration = new OneSignalApi.Client.Configuration()
        {
            BasePath = configuration.BasePath,
            AccessToken = configuration.AccessToken
        };

        var client = new DefaultApi(clientConfiguration);

        return client;
    }
}
