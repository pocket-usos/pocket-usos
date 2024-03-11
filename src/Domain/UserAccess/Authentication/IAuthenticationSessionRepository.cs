using App.Domain.BuildingBlocks;

namespace App.Domain.UserAccess.Authentication;

public interface IAuthenticationSessionRepository : IRepository
{
    Task AddAsync(AuthenticationSession session);
    
    Task<AuthenticationSession> GetByIdAsync(AuthenticationSessionId id);

    Task<AuthenticationSession?> GetByIdOrDefaultAsync(AuthenticationSessionId id);

    Task RemoveAsync(AuthenticationSessionId authenticationSessionId);
}
