using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Domain.UserAccess;

public class AuthenticationSessionRepository(Context context) : IAuthenticationSessionRepository
{
    public async Task AddAsync(AuthenticationSession session)
    {
        await context.AddAsync(session);
    }

    public async Task<AuthenticationSession> GetByIdAsync(AuthenticationSessionId id)
    {
        var session = await context.AuthenticationSessions.SingleOrDefaultAsync(s => s.Id == id);

        if (session is null)
        {
            throw new NotFoundRepositoryException<AuthenticationSession>(id.Value);
        }
        
        return session;
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
