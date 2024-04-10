using App.Application.Configuration.Queries;
using App.Domain.UserAccess.Authentication;

namespace App.Application.Notifications.GetOneSignalExternalId;

public class GetOneSignalExternalIdQueryHandler(IAuthenticationSessionRepository authenticationSessionRepository) : IQueryHandler<GetOneSignalExternalIdQuery, string>
{
    public async Task<string> Handle(GetOneSignalExternalIdQuery query, CancellationToken cancellationToken)
    {
        var sessionId = new AuthenticationSessionId(query.SessionId);

        var session = await authenticationSessionRepository.GetByIdAsync(sessionId);

        var plainExternalId = session.InstitutionId.Value.ToString() + ':' + session.UserId;
        var plainExternalIdBytes = System.Text.Encoding.UTF8.GetBytes(plainExternalId);

        return Convert.ToBase64String(plainExternalIdBytes);
    }
}
