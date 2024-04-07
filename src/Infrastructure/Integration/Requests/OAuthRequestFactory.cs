using App.Application.Configuration;
using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Integration.Configuration;
using OAuth;

namespace App.Infrastructure.Integration.Requests;

internal class OAuthRequestFactory(UsosClientConfiguration configuration, IExecutionContextAccessor context, IAuthenticationSessionRepository authenticationSessionRepository) : IAuthorizedRequestFactory
{
    public Request CreateAccessTokenRequest(string path, string requestToken, string requestTokenSecret, string verifier, Action<Request>? configureRequest = null)
    {
        var request = Request.Post(path);
        configureRequest?.Invoke(request);

        var oauthRequest = new OAuthRequest
        {
            Method = "POST",
            Type = OAuthRequestType.AccessToken,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = configuration.ConsumerKey,
            ConsumerSecret = configuration.ConsumerSecret,
            RequestUrl = configuration.BaseUrl + request.GetFullUrl(),
            Version = "1.0",
            Token = requestToken,
            TokenSecret = requestTokenSecret,
            Verifier = verifier
        };

        var oauthHeader = oauthRequest.GetAuthorizationHeader();
        request.Headers.Add(HeaderName.Authorization, oauthHeader);

        return request;
    }

    public Request CreateRequestTokenRequest(string path, Action<Request>? configureRequest = null)
    {
        var request = Request.Post(path);
        configureRequest?.Invoke(request);

        var oauthRequest = new OAuthRequest
        {
            Method = "POST",
            Type = OAuthRequestType.RequestToken,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = configuration.ConsumerKey,
            ConsumerSecret = configuration.ConsumerSecret,
            RequestUrl = configuration.BaseUrl + request.GetFullUrl(),
            Version = "1.0",
            CallbackUrl = configuration.CallbackUrl,
        };

        var oauthHeader = oauthRequest.GetAuthorizationHeader();
        request.Headers.Add(HeaderName.Authorization, oauthHeader);

        return request;
    }

    public async Task<Request> CreateGetRequestAsync(string path, Action<Request>? configureRequest = null)
    {
        var request = Request.Get(path);
        configureRequest?.Invoke(request);

        var sessionId = new AuthenticationSessionId(context.SessionId);
        var session = await authenticationSessionRepository.GetByIdAsync(sessionId);

        var oauthRequest = new OAuthRequest
        {
            Method = "GET",
            Type = OAuthRequestType.RequestToken,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = configuration.ConsumerKey,
            ConsumerSecret = configuration.ConsumerSecret,
            RequestUrl = path,
            Version = "1.0",
            Token = session.AccessToken?.Value,
            TokenSecret = session.AccessToken?.Secret
        };

        var oauthHeader = oauthRequest.GetAuthorizationHeader();
        request.Headers.Add(HeaderName.Authorization, oauthHeader);

        return request;
    }
}
