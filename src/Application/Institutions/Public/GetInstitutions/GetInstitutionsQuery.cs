using App.Application.Contracts;

namespace App.Application.Institutions.Public.GetInstitutions;

public class GetInstitutionsQuery : QueryBase<IEnumerable<InstitutionDto>>;
