using App.Application.Contracts;
using App.Infrastructure.Configuration.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure.Configuration;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("App");
        
        services.AddDataAccess(connectionString!);
        services.AddIntegration(configuration);
        services.AddMediatRForAssemblies(Assemblies.Application, Assemblies.Infrastructure);

        CompositionRoot.SetServiceProvider(services.BuildServiceProvider());

        services.AddScoped<IGateway, Gateway>();

        return services;
    }
}
