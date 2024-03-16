using App.Application.Configuration;
using App.Application.Exceptions;

namespace App.API.Configuration;

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

    public string Language
    {
        get
        {
            if (IsAvailable && httpContextAccessor.HttpContext!.Request.Headers.AcceptLanguage.Count > 0)
            {
                var acceptedLanguage = httpContextAccessor.HttpContext!.Request.Headers.AcceptLanguage.FirstOrDefault(SupportedCultures.Check);
                if (acceptedLanguage is not null)
                {
                    return acceptedLanguage;
                }
            }

            return SupportedCultures.Default;
        }
    }

    public bool IsAvailable => httpContextAccessor.HttpContext != null;
}
