namespace App.Infrastructure.Integration.Requests;

internal interface IAuthorizedRequestFactory
{
    Task<Request> CreateAccessTokenRequestAsync(string path, string requestToken, string requestTokenSecret, string verifier, Action<Request>? configureRequest = null);
    Task<Request> CreateRequestTokenRequestAsync(Guid institutionId, string path, Action<Request>? configureRequest = null);
    Task<Request> CreateGetRequestAsync(string path, Action<Request>? configureRequest = null, Guid? providedSessionId = null);
}
