using App.Application.Configuration;
using App.Application.Exceptions;

namespace App.API.Configuration;

public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    private Guid? _sessionId;

    public Guid SessionId
    {
        get
        {
            if (IsAvailable && httpContextAccessor.HttpContext!.Request.Headers.Keys.Any(
                    x => x.ToLower() == CustomHeaders.SessionId.ToLower()))
            {
                _sessionId = Guid.Parse(httpContextAccessor.HttpContext.Request.Headers[CustomHeaders.SessionId].ToString());

                return _sessionId.Value;
            }

            if (_sessionId is not null)
            {
                return _sessionId.Value;
            }

            throw new NotAuthenticatedException("Profile is not authenticated");
        }
        set
        {
            _sessionId = value;
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

    public string Host
    {
        get
        {
            if (IsAvailable)
            {
                var scheme = httpContextAccessor.HttpContext!.Request.Scheme;
                var host = httpContextAccessor.HttpContext!.Request.Host;

                return $"{scheme}://{host}";
            }

            throw new ApplicationException("Http Context is not available");
        }
    }

    public bool IsAvailable => httpContextAccessor.HttpContext != null;
}
