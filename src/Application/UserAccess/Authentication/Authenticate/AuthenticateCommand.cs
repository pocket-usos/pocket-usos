using App.Application.Contracts;

namespace App.Application.UserAccess.Authentication.Authenticate;

public class AuthenticateCommand(Guid sessionId, string requestToken, string verifier) : CommandBase
{
    public Guid SessionId { get; } = sessionId;
    
    public string RequestToken { get; } = requestToken;
    
    public string Verifier { get; } = verifier;
}
