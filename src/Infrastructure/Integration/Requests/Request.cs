using System.Net.Mime;
using System.Text;
using System.Text.Json;
using App.Infrastructure.Integration.Json;

namespace App.Infrastructure.Integration.Requests;

internal abstract class Request(HttpMethod method, Guid institutionId, string path)
{
    public HttpMethod Method { get; } = method;

    public Guid InstitutionId { get; } = institutionId;

    private string Path { get; } = path;

    private IDictionary<string, object> _queryParameters = new Dictionary<string, object>();

    public HttpContent? Content { get; protected set; }

    public Request WithQueryParameter(string key, object value)
    {
        _queryParameters[key] = value;

        return this;
    }

    public Request WithQueryParameters(IDictionary<string, object> queryParameters)
    {
        foreach (var queryParameter in queryParameters)
        {
            _queryParameters[queryParameter.Key] = queryParameter.Value;
        }

        return this;
    }

    public string GetFullUrl()
    {
        if (_queryParameters.Any())
        {
            var queryParameters = _queryParameters.Select(parameter => $"{parameter.Key}={parameter.Value}");

            return Path + "?" + string.Join("&", queryParameters);
        }

        return Path;
    }

    public static GetRequest Get(Guid institutionId, string path)
    {
        return new GetRequest(institutionId, path);
    }

    public static PostRequest Post(Guid institutionId, string path)
    {
        return new PostRequest(institutionId, path);
    }

    public static AccessTokenRequest AccessToken(Guid institutionId, string path, string requestToken, string requestTokenSecret, string verifier)
    {
        return new AccessTokenRequest(institutionId, path, requestToken, requestTokenSecret, verifier);
    }

    public static RequestTokenRequest RequestToken(Guid institutionId, string path)
    {
        return new RequestTokenRequest(institutionId, path);
    }

    public static PatchRequest Patch(Guid institutionId, string path)
    {
        return new PatchRequest(institutionId, path);
    }

    public static PutRequest Put(Guid institutionId, string path)
    {
        return new PutRequest(institutionId, path);
    }

    public static DeleteRequest Delete(Guid institutionId, string path)
    {
        return new DeleteRequest(institutionId, path);
    }
}

internal class GetRequest(Guid institutionId, string path) : Request(HttpMethod.Get, institutionId, path);

internal class PostRequest(Guid institutionId, string path) : Request(HttpMethod.Post, institutionId, path)
{
    public Request WithContent(object content)
    {
        Content = new StringContent(
            JsonSerializer.Serialize(new Dictionary<string, object> { { "data", content } }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            }),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        return this;
    }
}

internal class RequestTokenRequest(Guid institutionId, string path) : PostRequest(institutionId, path);

internal class AccessTokenRequest(Guid institutionId, string path, string requestToken, string requestTokenSecret, string verifier) : PostRequest(institutionId, path)
{
    public string Token { get; } = requestToken;

    public string Secret { get; } = requestTokenSecret;

    public string Verifier { get; } = verifier;
}

internal class PatchRequest(Guid institutionId, string path) : Request(HttpMethod.Patch, institutionId, path)
{
    public Request WithContent(object content)
    {
        Content = new StringContent(
            JsonSerializer.Serialize(new Dictionary<string, object> { { "data", content } }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            }),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        return this;
    }
}

internal class PutRequest(Guid institutionId, string path) : Request(HttpMethod.Put, institutionId, path)
{
    public Request WithContent(object content)
    {
        Content = new StringContent(
            JsonSerializer.Serialize(new Dictionary<string, object> { { "data", content } }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            }),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        return this;
    }
}

internal class DeleteRequest(Guid institutionId, string path) : Request(HttpMethod.Delete, institutionId, path);
