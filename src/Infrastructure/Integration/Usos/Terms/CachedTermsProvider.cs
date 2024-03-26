using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;

namespace App.Infrastructure.Integration.Usos.Terms;

internal class CachedTermsProvider(ITermsProvider termsProvider, ICacheProvider cache, IExecutionContextAccessor context) : ITermsProvider
{
    public async Task<TermsDto> GetTerms()
    {
        var terms = await cache.GetAsync<TermsDto>($"usos-user-terms-{context.SessionId}");

        if (terms is not null)
        {
            return terms;
        }

        terms = await termsProvider.GetTerms();

        await cache.SetAsync($"usos-user-terms-{context.SessionId}", terms, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return terms;
    }
}   
