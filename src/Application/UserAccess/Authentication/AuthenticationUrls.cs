namespace App.Application.UserAccess.Authentication;

public static class AuthenticationUrls
{
    public static string RequestTokenUrl => "services/oauth/request_token";
    
    public static string AuthorizeUrl => "services/oauth/authorize";
    
    public static string AccessTokenUrl => "services/oauth/access_token";
}
