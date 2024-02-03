namespace App.Domain.UserAccess.Authentication;

public class AuthenticationSession
{
    public AuthenticationSessionId Id { get; private set; }
    
    public RequestToken RequestToken { get; private set; }
    
    public AccessToken? AccessToken { get; private set; }

    public static async Task<AuthenticationSession> Initialise(IAuthenticationService authenticationService)
    {
        var requestToken = await authenticationService.RetrieveRequestToken();
        
        return new AuthenticationSession(requestToken);
    }
    
    public async Task Authenticate(string token, string verifier, IAuthenticationService authenticationService)
    {
        var accessToken = await authenticationService.RetrieveAccessToken(token, RequestToken.Secret, verifier);
        AccessToken = accessToken;
    }

    private AuthenticationSession(RequestToken requestToken)
    {
        Id = new AuthenticationSessionId(Guid.NewGuid());
        RequestToken = requestToken;
    }

    private AuthenticationSession() { }
}
