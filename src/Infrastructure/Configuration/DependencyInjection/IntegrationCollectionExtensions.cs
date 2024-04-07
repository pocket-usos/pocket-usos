using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Configuration.DataAccess;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Configuration;
using App.Infrastructure.Integration.Requests;
using App.Infrastructure.Integration.Usos;
using App.Infrastructure.Integration.Usos.Authentication;
using App.Infrastructure.Integration.Usos.Courses;
using App.Infrastructure.Integration.Usos.Grades;
using App.Infrastructure.Integration.Usos.Students;
using App.Infrastructure.Integration.Usos.Terms;
using App.Infrastructure.Integration.Usos.TimeTable;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace App.Infrastructure.Configuration.DependencyInjection;

public static class IntegrationCollectionExtensions
{
    public static IServiceCollection AddIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        var clientConfiguration = configuration.GetSection("Usos").Get<UsosClientConfiguration>()!;

<<<<<<< HEAD
        services.AddHttpClient<UsosHttpClient>(client =>
        {
            client.BaseAddress = new Uri(clientConfiguration.BaseUrl);
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .AddPolicyHandler(GetRetryPolicy());

        services.AddScoped<IUsosHttpClient>(provider =>
        {
            var usosHttpClient = provider.GetRequiredService<UsosHttpClient>();
            var logger = provider.GetRequiredService<ILogger>();

            return new LoggingUsosHttpClientDecorator(usosHttpClient, logger);
        });

        services.AddScoped<IAuthorizedRequestFactory, OAuthRequestFactory>();

        services.AddSingleton(clientConfiguration);
        services.AddSingleton(usosConfiguration);
=======
        services.AddHttpClient<IUsosHttpClient, UsosHttpClient>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy());

        services.AddSingleton(clientConfiguration);
        services.AddScoped<AuthenticationHeaderProvider>();
>>>>>>> 59eade041b1a63306aa0017932f7ef66580717b2

        services.AddScoped<IAuthenticationService, UsosAuthenticationService>();
        services.AddScoped<IUsersProvider, UsosUsersProvider>();

        services.AddScoped<UsosGradesProvider>();
        services.AddScoped<IGradesProvider>(services =>
        {
            var gradesProvider = services.GetRequiredService<UsosGradesProvider>();
            var cache = services.GetRequiredService<ICacheProvider>();
            var executionContext = services.GetRequiredService<IExecutionContextAccessor>();

            return new CachedGradesProvider(gradesProvider, cache, executionContext);
        });

        services.AddScoped<UsosCoursesProvider>();
        services.AddScoped<ICoursesProvider>(services =>
        {
            var courcesProvider = services.GetRequiredService<UsosCoursesProvider>();
            var cache = services.GetRequiredService<ICacheProvider>();
            var executionContext = services.GetRequiredService<IExecutionContextAccessor>();

            return new CachedCoursesProvider(courcesProvider, cache, executionContext);
        });

        services.AddScoped<UsosTimeTableProvider>();
        services.AddScoped<ITimeTableProvider>(services =>
        {
            var timeTableProvider = services.GetRequiredService<UsosTimeTableProvider>();
            var cache = services.GetRequiredService<ICacheProvider>();
            var executionContext = services.GetRequiredService<IExecutionContextAccessor>();

            return new CachedTimeTableProvider(timeTableProvider, cache, executionContext);
        });

        services.AddScoped<UsosTermsProvider>();
        services.AddScoped<ITermsProvider>(service =>
        {
            var termsProvider = service.GetRequiredService<UsosTermsProvider>();
            var cache = service.GetRequiredService<ICacheProvider>();
            var executionContext = service.GetRequiredService<IExecutionContextAccessor>();

            return new CachedTermsProvider(termsProvider, cache, executionContext);
        });

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
