using App.Application.Contracts;

namespace App.Application.UserAccess.Authentication.InitialiseAuthenticationSession;

public class InitialiseAuthenticationSessionCommand(Guid institutionId)
    : CommandBase<AuthenticationSessionInitialisationResult>
{
    public Guid InstitutionId { get; } = institutionId;
}
