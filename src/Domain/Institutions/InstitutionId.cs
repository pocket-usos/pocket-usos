using App.Domain.BuildingBlocks;

namespace App.Domain.Institutions;

public class InstitutionId(Guid value) : TypedIdValueBase(value);
