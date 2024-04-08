using Polly;

namespace App.Infrastructure.Integration.Usos.Terms;

public interface ITermsProvider
{
    Task<TermsDto> GetTerms();
}
