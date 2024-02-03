using App.Application.Configuration;
using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Integration.Requests;
using App.Infrastructure.Integration.Responses;
using Serilog;

namespace App.Infrastructure.Integration.Client;

internal class UsosHttpClient(HttpClient httpClient, ILogger logger, AuthenticationHeaderProvider authenticationHeaderProvider, IExecutionContextAccessor executionContextAccessor) : IUsosHttpClient
{
    public async Task<Response> SendAsync(Request request)
    {
        var httpRequestMessage = new HttpRequestMessage(
            request.Method,
            request.GetFullUrl())
        {
            Content = request.Content
        };

        var requestFullUrl = httpClient.BaseAddress + request.GetFullUrl();

        if (request is AccessTokenRequest accessTokenRequest)
        {
            httpRequestMessage.Headers.Add(HeaderName.Authorization, authenticationHeaderProvider.GetAuthorizationHeader(requestFullUrl, new AccessTokenRequestWithVerifier(accessTokenRequest.Token, accessTokenRequest.Secret, accessTokenRequest.Verifier)));
        }
        else if (request is RequestTokenRequest)
        {
            httpRequestMessage.Headers.Add(HeaderName.Authorization, authenticationHeaderProvider.GetAuthorizationHeader(requestFullUrl));
        }
        else
        {
            var authorizationHeader = await authenticationHeaderProvider.GetAuthorizationHeader(new AuthenticationSessionId(executionContextAccessor.SessionId),requestFullUrl);
            httpRequestMessage.Headers.Add(HeaderName.Authorization, authorizationHeader);
        }

        logger.Information("Attempting to perform {Method}:{Url} request", request.Method, requestFullUrl);

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        var response = await Response.From(httpResponseMessage);

        if (response.IsSuccessful())
        {
            logger.Information("Request {Method}:{Url} processed successfully", request.Method, requestFullUrl);
        }
        else
        {
            logger.Warning(
                "Request {Method}:{Url} failed with {Status} HTTP code: {Message}. Error: {@Error}",
                request.Method,
                requestFullUrl,
                response.StatusCode,
                response.Error?.Message,
                response.Error);
        }

        return await Response.From(httpResponseMessage);
    }
}
