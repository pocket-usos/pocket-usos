using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure.Configuration;

internal static class CompositionRoot
{
    private static IServiceProvider _serviceProvider = null!;

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static IServiceScope BeginScope()
    {
        return _serviceProvider.CreateScope();
    }
}