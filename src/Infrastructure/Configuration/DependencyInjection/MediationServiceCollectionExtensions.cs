using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure.Configuration.DependencyInjection;

public static class MediationServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRForAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies));

        return services;
    }
}
