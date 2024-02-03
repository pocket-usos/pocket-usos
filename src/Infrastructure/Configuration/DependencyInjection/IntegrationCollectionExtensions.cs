using App.Application.Configuration;
using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Configuration;
using App.Infrastructure.Integration.Usos;
using App.Infrastructure.Integration.Usos.Courses;
using App.Infrastructure.Integration.Usos.Grades;
using App.Infrastructure.Integration.Usos.Students;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace App.Infrastructure.Configuration.DependencyInjection;

public static class IntegrationCollectionExtensions
{
    public static IServiceCollection AddIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        var clientConfiguration = configuration.GetSection("Usos").Get<UsosClientConfiguration>()!;
        var usosConfiguration = configuration.GetSection("Usos").Get<UsosConfiguration>()!;

        services.AddHttpClient<IUsosHttpClient, UsosHttpClient>(client =>
            {
                client.BaseAddress = new Uri(clientConfiguration.BaseUrl);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy());

        services.AddSingleton(clientConfiguration);
        services.AddSingleton(usosConfiguration);
        services.AddScoped<AuthenticationHeaderProvider>();

        services.AddScoped<IAuthenticationService, Usos>();
        services.AddScoped<IUsersProvider, Usos>();
        services.AddScoped<IGradesProvider, Usos>();
        services.AddScoped<ICoursesProvider, Usos>();

        return services;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}
