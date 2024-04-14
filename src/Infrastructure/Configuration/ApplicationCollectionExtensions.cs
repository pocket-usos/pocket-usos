using App.Application.Contracts;
using App.Infrastructure.Configuration.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure.Configuration;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, string connectionString)
    {
        services.AddScoped<IGateway, Gateway>();

        services.AddDataAccess(connectionString);
        services.AddIntegration(configuration);
        services.AddMediatRForAssemblies(Assemblies.Application, Assemblies.Infrastructure);
        services.AddNotifications(configuration);

        CompositionRoot.SetServiceProvider(services.BuildServiceProvider());

        return services;
    }
}
