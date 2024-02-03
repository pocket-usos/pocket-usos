namespace App.Domain.UserAccess.Authentication;

public interface IAuthenticationService
{
    Task<RequestToken> RetrieveRequestToken();

    Task<AccessToken> RetrieveAccessToken(string token, string tokenSecret, string verifier);
}
