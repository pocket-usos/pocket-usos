using App.Application.Configuration.Commands;
using App.Domain.UserAccess.Authentication;

namespace App.Application.UserAccess.Authentication.LogOut;

internal class LogOutCommandHandler(IAuthenticationSessionRepository authenticationSessionRepository) : ICommandHandler<LogOutCommand>
{
    public async Task Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        await authenticationSessionRepository.RemoveAsync(new AuthenticationSessionId(request.SessionId));
        await authenticationSessionRepository.SaveAsync();
    }
}
