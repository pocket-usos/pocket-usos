using App.Infrastructure.Integration.Requests.Types;

namespace App.Infrastructure.Integration.Requests;

internal abstract class Request(HttpMethod method, string path)
{
    public HttpMethod Method { get; } = method;

    private string Path { get; } = path;

    private IDictionary<string, object> _queryParameters = new Dictionary<string, object>();

    public HttpContent? Content { get; protected set; }


    public IDictionary<string, string> Headers = new Dictionary<string, string>();

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

    public static GetRequest Get(string path)
    {
        return new GetRequest(path);
    }

    public static PostRequest Post(string path)
    {
        return new PostRequest(path);
    }

    public static PatchRequest Patch(string path)
    {
        return new PatchRequest(path);
    }

    public static PutRequest Put(string path)
    {
        return new PutRequest(path);
    }

    public static DeleteRequest Delete(string path)
    {
        return new DeleteRequest(path);
    }
}
