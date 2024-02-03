using App.Infrastructure.Configuration.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure.Configuration.DependencyInjection;

internal static class DataAccessServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddDbContext<Context>(options =>
        {
            options.UseNpgsql(databaseConnectionString);
            options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        });

        services.Scan(scan =>
        {
            scan.FromAssemblies(Assemblies.Infrastructure)
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        return services;
    }
}
