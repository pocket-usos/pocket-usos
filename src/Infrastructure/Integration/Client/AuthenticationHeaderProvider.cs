using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Integration.Configuration;
using OAuth;

namespace App.Infrastructure.Integration.Client;

internal class AuthenticationHeaderProvider(UsosClientConfiguration configuration, IAuthenticationSessionRepository authenticationSessionRepository)
{
    public string GetAuthorizationHeader(string requestUrl)
    {
        var oauthRequest = new OAuthRequest
        {
            Method = "POST",
            Type = OAuthRequestType.RequestToken,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = configuration.ConsumerKey,
            ConsumerSecret = configuration.ConsumerSecret,
            RequestUrl = requestUrl,
            Version = "1.0",
            CallbackUrl = configuration.CallbackUrl,
        };

        return oauthRequest.GetAuthorizationHeader();
    }

    public async Task<string> GetAuthorizationHeader(AuthenticationSessionId sessionId, string requestUrl)
    {
        var session = await authenticationSessionRepository.GetByIdAsync(sessionId);

        var oauthRequest = new OAuthRequest
        {
            Method = "GET",
            Type = OAuthRequestType.RequestToken,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = configuration.ConsumerKey,
            ConsumerSecret = configuration.ConsumerSecret,
            RequestUrl = requestUrl,
            Version = "1.0",
            Token = session.AccessToken?.Value,
            TokenSecret = session.AccessToken?.Secret
        };

        return oauthRequest.GetAuthorizationHeader();
    }

    public string GetAuthorizationHeader(string requestUrl, AccessTokenRequestWithVerifier accessToken)
    {
        var oauthRequest = new OAuthRequest
        {
            Method = "POST",
            Type = OAuthRequestType.AccessToken,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ConsumerKey = configuration.ConsumerKey,
            ConsumerSecret = configuration.ConsumerSecret,
            RequestUrl = requestUrl,
            Version = "1.0",
            Token = accessToken.Token,
            TokenSecret = accessToken.Secret,
            Verifier = accessToken.Verifier
        };

        return oauthRequest.GetAuthorizationHeader();
    }
}

internal class AccessTokenRequestWithVerifier(string token, string secret, string verifier)
{
    public string Token { get; } = token;

    public string Secret { get; } = secret;

    public string Verifier { get; } = verifier;
}
