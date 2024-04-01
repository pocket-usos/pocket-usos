using App.Application.Configuration;
using App.Application.Configuration.Commands;
using App.Application.Users;
using App.Domain.UserAccess.Authentication;

namespace App.Application.UserAccess.Authentication.Authenticate;

public class AuthenticateCommandHandler(
    IAuthenticationSessionRepository authenticationSessionRepository,
    IAuthenticationService authenticationService,
    IExecutionContextAccessor context,
    IUserRepository userRepository) : ICommandHandler<AuthenticateCommand>
{
    public async Task Handle(AuthenticateCommand command, CancellationToken cancellationToken)
    {
        var session = await authenticationSessionRepository.GetByIdAsync(new AuthenticationSessionId(command.SessionId));

        await session.Authenticate(command.RequestToken, command.Verifier, authenticationService);
        await authenticationSessionRepository.SaveAsync();

        context.SessionId = session.Id.Value;

        var user = await userRepository.GetCurrentAsync();
        session.SetUserId(user.Id);
        await authenticationSessionRepository.SaveAsync();
    }
}
