using System.Net;
using System.Text.Json;
using App.Infrastructure.Integration.Exceptions;
using App.Infrastructure.Integration.Json;

namespace App.Infrastructure.Integration.Responses;

internal class Response
{
    public HttpStatusCode StatusCode { get; }

    public ResponseContent? Content { get; private set; }

    public ResponseError? Error { get; private set; }

    private Response(HttpStatusCode statusCode, ResponseContent? content)
    {
        StatusCode = statusCode;
        Content = content;
    }

    private Response(HttpStatusCode statusCode, ResponseError? error)
    {
        StatusCode = statusCode;
        Error = error;
    }

    public static async Task<Response> From(HttpResponseMessage responseMessage)
    {
        if (!responseMessage.IsSuccessStatusCode)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var responseError = JsonSerializer.Deserialize<ResponseError>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            });

            if (responseError is null)
            {
                throw new InvalidResponseContentException("Response notificationContent was not properly deserialized");
            }

            return new Response(responseMessage.StatusCode, responseError);
        }

        var serializedResponseContent = await responseMessage.Content.ReadAsStringAsync();
        var content = ResponseContent.From(serializedResponseContent);

        return new Response(responseMessage.StatusCode, content);
    }

    public UsosIntegrationException ToException(string language)
    {
        if (!IsSuccessful() && Error is not null)
        {
            return new UsosIntegrationException(Error.Message, Error.GetUserMessage(language), StatusCode);
        }

        throw new InvalidOperationException("Successful response cannot be converted to exception");
    }

    public bool IsSuccessful() => (int)StatusCode >= 200 && (int)StatusCode <= 299;
}
