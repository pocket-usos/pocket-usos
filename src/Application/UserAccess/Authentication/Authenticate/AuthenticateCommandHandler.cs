using App.Application.Configuration.Commands;
using App.Domain.UserAccess.Authentication;

namespace App.Application.UserAccess.Authentication.Authenticate;

public class AuthenticateCommandHandler(IAuthenticationSessionRepository authenticationSessionRepository, IAuthenticationService authenticationService) : ICommandHandler<AuthenticateCommand>
{
    public async Task Handle(AuthenticateCommand command, CancellationToken cancellationToken)
    {
        var session = await authenticationSessionRepository.GetByIdAsync(new AuthenticationSessionId(command.SessionId));
        
        await session.Authenticate(command.RequestToken, command.Verifier, authenticationService);
        await authenticationSessionRepository.SaveAsync();
    }
}