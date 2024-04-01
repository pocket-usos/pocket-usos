using App.Domain.Institutions;

namespace App.Domain.UserAccess.Authentication;

public class AuthenticationSession
{
    public AuthenticationSessionId Id { get; private set; }

    public string? UserId { get; private set; }

    public InstitutionId InstitutionId { get; private set; }

    public RequestToken RequestToken { get; private set; }

    public AccessToken? AccessToken { get; private set; }

    public static async Task<AuthenticationSession> Initialise(InstitutionId institutionId, IAuthenticationService authenticationService)
    {
        var requestToken = await authenticationService.RetrieveRequestToken(institutionId.Value);

        return new AuthenticationSession(institutionId, requestToken);
    }

    public async Task Authenticate(string token, string verifier, IAuthenticationService authenticationService)
    {
        var accessToken = await authenticationService.RetrieveAccessToken(token, RequestToken.Secret, verifier);
        AccessToken = accessToken;
    }

    public void SetUserId(string userId)
    {
        UserId = userId;
    }

    private AuthenticationSession(InstitutionId institutionId, RequestToken requestToken)
    {
        Id = new AuthenticationSessionId(Guid.NewGuid());
        InstitutionId = institutionId;
        RequestToken = requestToken;
    }

    private AuthenticationSession() { }
}
