namespace App.Application.Configuration;

public interface IExecutionContextAccessor
{
    Guid SessionId { get; }

    string? Language { get; }

    bool IsAvailable { get; }
}
