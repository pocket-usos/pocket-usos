using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Domain.UserAccess;

public class AuthenticationSessionRepository(Context context) : IAuthenticationSessionRepository
{
    private AuthenticationSession? _cachedAuthenticationSession;

    public async Task AddAsync(AuthenticationSession session)
    {
        await context.AddAsync(session);
    }

    public async Task<AuthenticationSession> GetByIdAsync(AuthenticationSessionId id)
    {
        if (_cachedAuthenticationSession is not null)
        {
            return _cachedAuthenticationSession;
        }

        var session = await context.AuthenticationSessions.SingleOrDefaultAsync(s => s.Id == id);

        if (session is null)
        {
            throw new NotFoundRepositoryException<AuthenticationSession>(id.Value);
        }

        return _cachedAuthenticationSession = session;
    }

    public async Task<AuthenticationSession?> GetByIdOrDefaultAsync(AuthenticationSessionId id)
    {
        if (_cachedAuthenticationSession is not null)
        {
            return _cachedAuthenticationSession;
        }

        return _cachedAuthenticationSession = await context.AuthenticationSessions.SingleOrDefaultAsync(s => s.Id == id);
    }

    public async Task<AuthenticationSession?> GetByUserIdOrDefaultAsync(string userId)
    {
        return await context.AuthenticationSessions.FirstOrDefaultAsync(s => s.UserId == userId);
    }

    public async Task RemoveAsync(AuthenticationSessionId authenticationSessionId)
    {
        var authenticationSession = await context.AuthenticationSessions.FindAsync(authenticationSessionId);
        if (authenticationSession is null)
        {
            return;
        }

        context.Remove(authenticationSession);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
