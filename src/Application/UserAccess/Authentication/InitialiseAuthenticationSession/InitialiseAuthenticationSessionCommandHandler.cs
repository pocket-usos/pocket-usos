using System.Text;
using App.Application.Configuration.Commands;
using App.Domain.Institutions;
using App.Domain.UserAccess.Authentication;

namespace App.Application.UserAccess.Authentication.InitialiseAuthenticationSession;

internal class InitialiseAuthenticationSessionCommandHandler(
    IAuthenticationService authenticationService,
    IAuthenticationSessionRepository authenticationSessionRepository,
    IInstitutionRepository institutionRepository) : ICommandHandler<InitialiseAuthenticationSessionCommand, AuthenticationSessionInitialisationResult>
{
    public async Task<AuthenticationSessionInitialisationResult> Handle(InitialiseAuthenticationSessionCommand command, CancellationToken cancellationToken)
    {
        var institutionId = new InstitutionId(command.InstitutionId);
        var authenticationSession = await AuthenticationSession.Initialise(institutionId, authenticationService);

        await authenticationSessionRepository.AddAsync(authenticationSession);
        await authenticationSessionRepository.SaveAsync();

        var redirectUrl = await BuildRedirectUrl(institutionId, authenticationSession.RequestToken);

        return new AuthenticationSessionInitialisationResult(authenticationSession.Id.Value, redirectUrl);
    }

    private async Task<string> BuildRedirectUrl(InstitutionId institutionId, RequestToken requestToken)
    {
        var institution = await institutionRepository.GetByIdAsync(institutionId);

        var redirectUrl = new StringBuilder(institution.BaseUrl)
            .Append(AuthenticationUrls.AuthorizeUrl)
            .Append("?oauth_token=")
            .Append(requestToken.Value)
            .ToString();

        return redirectUrl;
    }
}
