namespace App.Infrastructure.Integration.Responses;

internal class ResponseError(string message, string error, string reason, UserMessages? userMessages)
{
    public string Message { get; } = message;

    public string Error { get; } = error;

    public string Reason { get; } = reason;

    public UserMessages? UserMessages { get; set; } = userMessages;

    public string? GetUserMessage(string language)
    {
        return UserMessages?.GenericMessage?[language];
    }
}

internal class UserMessages(IDictionary<string, string> genericMessage, IDictionary<string, IDictionary<string, string>>? fields)
{
    public IDictionary<string, string>? GenericMessage = genericMessage;

    public IDictionary<string, IDictionary<string, string>>? Fields = fields;
}
