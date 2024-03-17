using System.Net;

namespace App.Infrastructure.Integration.Exceptions;

public class UsosIntegrationException(string message, string? userMessage, HttpStatusCode statusCode) : Exception(message)
{
    public string? UserMessage { get; } = userMessage;

    public HttpStatusCode StatusCode { get; } = statusCode;
}
