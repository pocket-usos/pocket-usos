using App.Domain.UserAccess.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.API.Configuration.UsosAuthorize;

internal class UsosAuthorizeFilter(IAuthenticationSessionRepository authenticationSessionRepository) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var sessionIdValue = context.HttpContext.Request.Headers[CustomHeaders.SessionId].FirstOrDefault();
        if (sessionIdValue is null || !Guid.TryParse(sessionIdValue, out var sessionId))
        {
            context.Result = Error(UnauthorizedProblemDetails.SessionIdIsNotProvidedOrInvalid);
            return;
        }

        var session = await authenticationSessionRepository.GetByIdOrDefaultAsync(new AuthenticationSessionId(sessionId));

        if (session is null)
        {
            context.Result = Error(UnauthorizedProblemDetails.SessionDoesNotExist);
            return;
        }

        if (session.AccessToken is null)
        {
            context.Result = Error(UnauthorizedProblemDetails.SessionIsNotAthorizedInUsos);
        }
    }

    private static IActionResult Error(ProblemDetails problemDetails) => new ObjectResult(problemDetails);
}
