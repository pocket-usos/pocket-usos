namespace App.Infrastructure.Integration.Requests;

internal interface IAuthorizedRequestFactory
{
    Request CreateAccessTokenRequest(string path, string requestToken, string requestTokenSecret, string verifier, Action<Request>? configureRequest = null);
    Request CreateRequestTokenRequest(string path, Action<Request>? configureRequest = null);
    Task<Request> CreateGetRequestAsync(string path, Action<Request>? configureRequest = null);
}
