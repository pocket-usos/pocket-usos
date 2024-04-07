using App.Application.Configuration;
using App.Domain.Institutions;
using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Integration.Configuration;
using OAuth;

namespace App.Infrastructure.Integration.Requests;

internal class OAuthRequestFactory(UsosClientConfiguration configuration, IExecutionContextAccessor context, IAuthenticationSessionRepository authenticationSessionRepository, IInstitutionRepository institutionRepository) : IAuthorizedRequestFactory
{
    public async Task<Request> CreateAccessTokenRequestAsync(string path, string requestToken, string requestTokenSecret, string verifier, Action<Request>? configureRequest = null)
    {
        var sessionId = new AuthenticationSessionId(context.SessionId);
        var session = await authenticationSessionRepository.GetByIdAsync(sessionId);

        var institutionConfiguration = configuration.Institutions[session.InstitutionId.ToString()];
        var institution = await institutionRepository.GetByIdAsync(session.InstitutionId);

        var fullPath = institution.BaseUrl + path;
        var request = Request.Post(fullPath);
        configureRequest?.Invoke(request);

        var oauthRequest = new OAuthRequest
        {
            Method = "POST",
            Type = OAuthRequestType.AccessToken,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = institutionConfiguration.ConsumerKey,
            ConsumerSecret = institutionConfiguration.ConsumerSecret,
            RequestUrl = request.GetFullUrl(),
            Version = "1.0",
            Token = requestToken,
            TokenSecret = requestTokenSecret,
            Verifier = verifier
        };

        var oauthHeader = oauthRequest.GetAuthorizationHeader();
        request.Headers.Add(HeaderName.Authorization, oauthHeader);

        return request;
    }

    public async Task<Request> CreateRequestTokenRequestAsync(Guid institutionId, string path, Action<Request>? configureRequest = null)
    {
        var institutionConfiguration = configuration.Institutions[institutionId.ToString()];
        var institution = await institutionRepository.GetByIdAsync(new InstitutionId(institutionId));

        var fullPath = institution.BaseUrl + path;
        var request = Request.Post(fullPath);
        configureRequest?.Invoke(request);

        var oauthRequest = new OAuthRequest
        {
            Method = "POST",
            Type = OAuthRequestType.RequestToken,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = institutionConfiguration.ConsumerKey,
            ConsumerSecret = institutionConfiguration.ConsumerSecret,
            RequestUrl = request.GetFullUrl(),
            Version = "1.0",
            CallbackUrl = configuration.CallbackUrl,
        };

        var oauthHeader = oauthRequest.GetAuthorizationHeader();
        request.Headers.Add(HeaderName.Authorization, oauthHeader);

        return request;
    }

    public async Task<Request> CreateGetRequestAsync(string path, Action<Request>? configureRequest = null)
    {
        var sessionId = new AuthenticationSessionId(context.SessionId);
        var session = await authenticationSessionRepository.GetByIdAsync(sessionId);

        var institutionConfiguration = configuration.Institutions[session.InstitutionId.ToString()];
        var institution = await institutionRepository.GetByIdAsync(session.InstitutionId);

        var fullPath = institution.BaseUrl + path;
        var request = Request.Get(fullPath);
        configureRequest?.Invoke(request);

        var oauthRequest = new OAuthRequest
        {
            Method = "GET",
            Type = OAuthRequestType.RequestToken,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = institutionConfiguration.ConsumerKey,
            ConsumerSecret = institutionConfiguration.ConsumerSecret,
            RequestUrl = request.GetFullUrl(),
            Version = "1.0",
            Token = session.AccessToken?.Value,
            TokenSecret = session.AccessToken?.Secret
        };

        var oauthHeader = oauthRequest.GetAuthorizationHeader();
        request.Headers.Add(HeaderName.Authorization, oauthHeader);

        return request;
    }
}
