using App.Application.Configuration;
using App.Application.Configuration.Queries;
using App.Domain.Institutions;

namespace App.Application.Institutions.Public.GetInstitutions;

public class GetInstitutionsQueryHandler(IInstitutionRepository institutionRepository, IExecutionContextAccessor context) : IQueryHandler<GetInstitutionsQuery, IEnumerable<InstitutionDto>>
{
    public async Task<IEnumerable<InstitutionDto>> Handle(GetInstitutionsQuery query, CancellationToken cancellationToken)
    {
        var institutions = await institutionRepository.GetEnabledAsync();

        return institutions.Select(i => new InstitutionDto
        {
            Id = i.Id.Value,
            Name = context.Language == "pl" ? i.Name.PlValue : i.Name.EnValue,
            LogoUrl = context.Host + (i.LogoPath ?? Institution.DefaultLogoPath),
            IsBeta = i.IsBeta,
        });
    }
}
