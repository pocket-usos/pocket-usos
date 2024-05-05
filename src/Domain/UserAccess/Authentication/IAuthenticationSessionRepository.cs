using App.Domain.BuildingBlocks;
using App.Domain.Institutions;

namespace App.Domain.UserAccess.Authentication;

public interface IAuthenticationSessionRepository : IRepository
{
    Task AddAsync(AuthenticationSession session);

    Task<AuthenticationSession> GetByIdAsync(AuthenticationSessionId id);

    Task<AuthenticationSession?> GetByIdOrDefaultAsync(AuthenticationSessionId id);

    Task<AuthenticationSession?> GetByUserIdAndInstitutionIdOrDefaultAsync(string userId, InstitutionId institutionId);

    Task RemoveAsync(AuthenticationSessionId authenticationSessionId);
}
