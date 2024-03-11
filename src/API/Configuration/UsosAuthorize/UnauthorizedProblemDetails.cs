using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.UsosAuthorize;

internal static class UnauthorizedProblemDetails
{
    public static ProblemDetails SessionIdIsNotProvidedOrInvalid => Create($"{CustomHeaders.SessionId} is not provided or invalid");

    public static ProblemDetails SessionDoesNotExist => Create("Provided session id does not exist");

    public static ProblemDetails SessionIsNotAthorizedInUsos => Create("Provided session id is not authorized in USOS");

    private static ProblemDetails Create(string detail) => new ProblemDetails()
    {
        Type = @"https://tools.ietf.org/html/rfc9110#section-15.5.2",
        Status = StatusCodes.Status401Unauthorized,
        Title = "Authorization Failed",
        Detail = detail
    };
}
