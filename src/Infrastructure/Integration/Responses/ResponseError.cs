namespace App.Infrastructure.Integration.Responses;

internal class ResponseError(string message, string error, string reason)
{
    public string Message { get; } = message;

    public string Error { get; } = error;

    public string Reason { get; } = reason;
}
