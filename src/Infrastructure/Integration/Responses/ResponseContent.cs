using System.Text.Json;
using App.Infrastructure.Integration.Exceptions;
using App.Infrastructure.Integration.Json;

namespace App.Infrastructure.Integration.Responses;

internal class ResponseContent
{
    private readonly string _serializedContent;

    public T As<T>()
    {
        var responseContentData = JsonSerializer.Deserialize<T>(_serializedContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        });

        if (responseContentData is null)
        {
            throw new InvalidResponseContentException("Response content was not properly deserialized");
        }

        return responseContentData;
    }

    public string AsString()
    {
        return _serializedContent;
    }

    public static ResponseContent From(string serializedContent)
    {
        return new ResponseContent(serializedContent);
    }

    private ResponseContent(string serializedContent)
    {
        _serializedContent = serializedContent;
    }
}
