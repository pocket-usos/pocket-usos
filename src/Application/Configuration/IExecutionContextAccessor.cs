namespace App.Application.Configuration;

public interface IExecutionContextAccessor
{
    Guid SessionId { get; set; }

    string Language { get; }

    string Host { get; }

    bool IsAvailable { get; }
}
