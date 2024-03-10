using App.Application.Configuration;
using App.Application.Exceptions;

namespace App.API.Configuration.ExecutionContext;

public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    private const string SessionIdHeaderName = "Session-Id";

    public Guid SessionId
    {
        get
        {
            if (IsAvailable && httpContextAccessor.HttpContext!.Request.Headers.Keys.Any(
                    x => x.ToLower() == SessionIdHeaderName.ToLower()))
            {
                return Guid.Parse(httpContextAccessor.HttpContext.Request.Headers[SessionIdHeaderName].ToString());
            }

            throw new NotAuthenticatedException("Profile is not authenticated");
        }
    }

    public string? Language
    {
        get
        {
            if (IsAvailable && httpContextAccessor.HttpContext!.Request.Headers.AcceptLanguage.Count > 0)
            {
                return httpContextAccessor.HttpContext!.Request.Headers.AcceptLanguage.FirstOrDefault(SupportedCultures.Check);
            }

            return null;
        }
    }

    public bool IsAvailable => httpContextAccessor.HttpContext != null;
}
