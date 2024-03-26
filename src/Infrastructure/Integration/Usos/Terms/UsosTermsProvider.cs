using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;

namespace App.Infrastructure.Integration.Usos.Terms;

internal class UsosTermsProvider(IUsosHttpClient client, IExecutionContextAccessor context, ICacheProvider cache) : ITermsProvider
{
    public async Task<TermsDto> GetTerms()
    {
        var terms = await cache.GetAsync<TermsDto>($"usos-user-terms-{context.SessionId}");

        if (terms is not null)
        {
            return terms;
        }

        var request = Request.Get("services/courses/user")
            .WithQueryParameter("fields", "terms");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }
        terms = response.Content!.As<TermsDto>();

        await cache.SetAsync($"usos-user-terms-{context.SessionId}", terms, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return terms;
    }
}
