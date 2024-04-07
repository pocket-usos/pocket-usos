using App.Application.Configuration;
using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;
using Microsoft.AspNetCore.WebUtilities;

namespace App.Infrastructure.Integration.Usos.Authentication;
internal class UsosAuthenticationService(IUsosHttpClient client, IAuthorizedRequestFactory requestFactory, IExecutionContextAccessor context) : IAuthenticationService
{
    public async Task<AccessToken> RetrieveAccessToken(string token, string tokenSecret, string verifier)
    {
        var request = requestFactory.CreateAccessTokenRequest("services/oauth/access_token", token, tokenSecret, verifier);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful() || response.Content is null)
        {
            throw response.ToException(context.Language);
        }

        var formReader = new FormReader(response.Content.AsString());
        var form = formReader.ReadForm();

        return new AccessToken(form["oauth_token"]!, form["oauth_token_secret"]!);
    }

    public async Task<RequestToken> RetrieveRequestToken()
    {
        var request = requestFactory.CreateRequestTokenRequest(
            "services/oauth/request_token",
            r => r.WithQueryParameter("scopes", String.Join('|', Scope.AllValues)));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful() || response.Content is null)
        {
            throw response.ToException(context.Language);
        }

        var formReader = new FormReader(response.Content.AsString());
        var form = formReader.ReadForm();

        return new RequestToken(form["oauth_token"]!, form["oauth_token_secret"]!);
    }
}
