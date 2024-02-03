namespace App.Application.UserAccess.Authentication.InitialiseAuthenticationSession;

public class AuthenticationSessionInitialisationResult(Guid sessionId, string redirectUrl)
{
    public Guid SessionId { get; set; } = sessionId;

    public string RedirectUrl { get; set; } = redirectUrl;
}
