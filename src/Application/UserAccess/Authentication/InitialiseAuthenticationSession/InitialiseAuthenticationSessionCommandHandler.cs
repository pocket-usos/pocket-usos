using System.Text;
using App.Application.Configuration;
using App.Application.Configuration.Commands;
using App.Domain.UserAccess.Authentication;

namespace App.Application.UserAccess.Authentication.InitialiseAuthenticationSession;

internal class InitialiseAuthenticationSessionCommandHandler(
    IAuthenticationService authenticationService,
    IAuthenticationSessionRepository authenticationSessionRepository,
    UsosConfiguration usosConfiguration) : ICommandHandler<InitialiseAuthenticationSessionCommand, AuthenticationSessionInitialisationResult>
{
    public async Task<AuthenticationSessionInitialisationResult> Handle(InitialiseAuthenticationSessionCommand sessionCommand, CancellationToken cancellationToken)
    {
        var authenticationSession = await AuthenticationSession.Initialise(authenticationService);
        
        await authenticationSessionRepository.AddAsync(authenticationSession);
        await authenticationSessionRepository.SaveAsync();

        var redirectUrl = BuildRedirectUrl(authenticationSession.RequestToken);

        return new AuthenticationSessionInitialisationResult(authenticationSession.Id.Value, redirectUrl);
    }

    private string BuildRedirectUrl(RequestToken requestToken)
    {
        var redirectUrl = new StringBuilder(usosConfiguration.BaseUrl)
            .Append(AuthenticationUrls.AuthorizeUrl)
            .Append("?oauth_token=")
            .Append(requestToken.Value)
            .ToString();
        
        return redirectUrl;
    }
}
