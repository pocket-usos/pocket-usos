using App.Application.Configuration;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;

namespace App.Infrastructure.Integration.Usos.Terms;

internal class UsosTermsProvider(IUsosHttpClient client, IAuthorizedRequestFactory requestFactory, IExecutionContextAccessor context) : ITermsProvider
{
    public async Task<TermsDto> GetTerms()
    {
        var request = await requestFactory.CreateGetRequestAsync("services/courses/user",
            r => r.WithQueryParameter("fields", "terms"));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<TermsDto>();
    }
}
