namespace App.Application.Configuration;

public interface IExecutionContextAccessor
{
    Guid SessionId { get; }

    bool IsAvailable { get; }
}
