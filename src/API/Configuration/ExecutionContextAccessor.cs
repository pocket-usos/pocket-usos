using App.Application.Configuration;
using App.Application.Exceptions;

namespace App.API.Configuration.ExecutionContext;

public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    public Guid SessionId
    {
        get
        {
            if (IsAvailable && httpContextAccessor.HttpContext!.Request.Headers.Keys.Any(
                    x => x.ToLower() == CustomHeaders.SessionId.ToLower()))
            {
                return Guid.Parse(httpContextAccessor.HttpContext.Request.Headers[CustomHeaders.SessionId].ToString());
            }

            throw new NotAuthenticatedException("Profile is not authenticated");
        }
    }

    public bool IsAvailable => httpContextAccessor.HttpContext != null;
}
