using App.Domain.BuildingBlocks;

namespace App.Domain.Institutions;

public interface IInstitutionRepository : IRepository
{
    Task AddAsync(Institution institution);

    Task<Institution> GetByIdAsync(InstitutionId id);

    Task<IEnumerable<Institution>> GetEnabledAsync();

    Task<IEnumerable<Institution>> GetAllAsync();
}
