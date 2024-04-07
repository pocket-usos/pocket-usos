<<<<<<< HEAD
=======
using App.Application.Configuration;
using App.Domain.Institutions;
using App.Domain.UserAccess.Authentication;
>>>>>>> 59eade041b1a63306aa0017932f7ef66580717b2
using App.Infrastructure.Integration.Requests;
using App.Infrastructure.Integration.Responses;

namespace App.Infrastructure.Integration.Client;

<<<<<<< HEAD
internal class UsosHttpClient(HttpClient httpClient) : IUsosHttpClient
=======
internal class UsosHttpClient(
    HttpClient httpClient,
    ILogger logger,
    AuthenticationHeaderProvider authenticationHeaderProvider,
    IExecutionContextAccessor executionContextAccessor,
    IInstitutionRepository institutionRepository) : IUsosHttpClient
>>>>>>> 59eade041b1a63306aa0017932f7ef66580717b2
{
    public async Task<Response> SendAsync(Request request)
    {
        var requestFullUrl = await GetBaseUrl(request.InstitutionId) + request.GetFullUrl();

        var httpRequestMessage = new HttpRequestMessage(request.Method, requestFullUrl)
        {
            Content = request.Content
        };

<<<<<<< HEAD
        foreach (var header in request.Headers)
        {
            httpRequestMessage.Headers.Add(header.Key, header.Value);
        }
=======
        if (request is AccessTokenRequest accessTokenRequest)
        {
            httpRequestMessage.Headers.Add(HeaderName.Authorization, authenticationHeaderProvider.GetAuthorizationHeader(request.InstitutionId, requestFullUrl, new AccessTokenRequestWithVerifier(accessTokenRequest.Token, accessTokenRequest.Secret, accessTokenRequest.Verifier)));
        }
        else if (request is RequestTokenRequest)
        {
            httpRequestMessage.Headers.Add(HeaderName.Authorization, authenticationHeaderProvider.GetAuthorizationHeader(request.InstitutionId, requestFullUrl));
        }
        else
        {
            var authorizationHeader = await authenticationHeaderProvider.GetAuthorizationHeader(new AuthenticationSessionId(executionContextAccessor.SessionId), requestFullUrl);
            httpRequestMessage.Headers.Add(HeaderName.Authorization, authorizationHeader);
        }

        logger.Information("Attempting to perform {Method}:{Url} request", request.Method, requestFullUrl);
>>>>>>> 59eade041b1a63306aa0017932f7ef66580717b2

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        return await Response.From(httpResponseMessage);
    }

    private async Task<string> GetBaseUrl(Guid institutionId)
    {
        var institution = await institutionRepository.GetByIdAsync(new InstitutionId(institutionId));

        return institution.BaseUrl;
    }
}
