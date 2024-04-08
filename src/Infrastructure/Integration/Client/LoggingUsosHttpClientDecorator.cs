using App.Infrastructure.Integration.Requests;
using App.Infrastructure.Integration.Responses;
using Serilog;

namespace App.Infrastructure.Integration.Client;
internal class LoggingUsosHttpClientDecorator : IUsosHttpClient
{
    private readonly IUsosHttpClient _usosHttpClient;
    private readonly ILogger _logger;

    public LoggingUsosHttpClientDecorator(IUsosHttpClient usosHttpClient, ILogger logger)
    {
        _usosHttpClient = usosHttpClient;
        _logger = logger;
    }

    public async Task<Response> SendAsync(Request request)
    {
        _logger.Information("Attempting to perform {Method}:{Url} request", request.Method, request.GetFullUrl());

        var response = await _usosHttpClient.SendAsync(request);

        if (response.IsSuccessful())
        {
            _logger.Information("Request {Method}:{Url} processed successfully", request.Method, request.GetFullUrl());
        }
        else
        {
            _logger.Warning(
                "Request {Method}:{Url} failed with {Status} HTTP code: {Message}. Error: {@Error}",
                request.Method,
                request.GetFullUrl(),
                response.StatusCode,
                response.Error?.Message,
                response.Error);
        }

        return response;
    }
}
