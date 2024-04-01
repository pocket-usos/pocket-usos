using App.Domain.BuildingBlocks;

namespace App.Domain.UserAccess.Authentication;

public interface IAuthenticationSessionRepository : IRepository
{
    Task AddAsync(AuthenticationSession session);

    Task<AuthenticationSession> GetByIdAsync(AuthenticationSessionId id);

    Task<AuthenticationSession?> GetByIdOrDefaultAsync(AuthenticationSessionId id);

    Task<AuthenticationSession?> GetByUserIdOrDefaultAsync(string userId);

    Task RemoveAsync(AuthenticationSessionId authenticationSessionId);
}
