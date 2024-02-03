namespace App.API.UserAccess.Requests;

public class AuthenticateRequest
{
    public string RequestToken { get; set; }
    
    public string Verifier { get; set; }
}
