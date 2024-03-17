using System.Net;
using App.Infrastructure.Integration.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Integration;

internal class UsosIntegrationProblemDetails : ProblemDetails
{
    public string? UserMessage { get; private set; }

    public UsosIntegrationProblemDetails(UsosIntegrationException exception)
    {
        Title = "USOS integration error";
        Status = exception.StatusCode == HttpStatusCode.Unauthorized ? StatusCodes.Status401Unauthorized : StatusCodes.Status424FailedDependency;
        Detail = exception.Message;
        UserMessage = exception.UserMessage;
    }
}
