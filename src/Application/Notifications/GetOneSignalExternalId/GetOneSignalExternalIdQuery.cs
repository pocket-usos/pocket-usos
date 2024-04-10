using App.Application.Contracts;

namespace App.Application.Notifications.GetOneSignalExternalId;

public class GetOneSignalExternalIdQuery(Guid sessionId) : QueryBase<string>
{
    public Guid SessionId { get; } = sessionId;
}
