using App.Application.Configuration;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;

namespace App.Infrastructure.Integration.Usos.Terms;

internal class UsosTermsProvider(IUsosHttpClient client, IExecutionContextAccessor context) : ITermsProvider
{
    public async Task<TermsDto> GetTerms()
    {
        var request = Request.Get("services/courses/user")
            .WithQueryParameter("fields", "terms");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<TermsDto>();
    }
}
