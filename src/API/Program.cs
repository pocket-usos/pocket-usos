using App.API.Configuration;
using App.API.Configuration.ExecutionContext;
using App.Application.Configuration;
using App.Infrastructure.Configuration;
using Destructurama;
using Serilog;
using Serilog.Enrichers.Sensitive;
using ILogger = Serilog.ILogger;

namespace App.API;

public class Program
{
    private static ILogger _logger;

    private static LoggerConfiguration _loggerConfiguration;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogger(builder.Environment);
        builder.Services.AddSingleton(_logger);

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(new NotFoundActionFilterAttribute());
        });
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSingleton<IServiceProvider>(provider => provider);
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

        builder.Services.AddLocalization();
        builder.Services.AddDistributedMemoryCache();
        // builder.Services.AddSingleton<ILocalizerFactory, JsonStringLocalizerFactory>();

        // builder.Services.AddProblemDetails(x =>
        // {
        // });
        
        builder.Services.AddApplicationServices(builder.Configuration);

        var app = builder.Build();

        // TODO: change for production
        app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        app.UseHttpsRedirection();
        app.UseRouting();

        var supportedCultures = new[] { "en", "pl", "ua" };
        var localizationOptions = new RequestLocalizationOptions()
        {
            ApplyCurrentCultureToResponseHeaders = true
        }
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);
        // app.UseProblemDetails();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.Run();
    }

    private static void ConfigureLogger(IWebHostEnvironment environment)
    {
        var logTemplate = "[{Environment}] [{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        _loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithSensitiveDataMasking(_ => { })
            .Destructure.UsingAttributes()
            .Enrich.WithProperty("Environment", environment.EnvironmentName);

        if (environment.IsDevelopment() || environment.IsProduction())
        {
            _loggerConfiguration = _loggerConfiguration.WriteTo.Console(outputTemplate: logTemplate);
        }

        _logger = _loggerConfiguration.CreateLogger();
    }
}
