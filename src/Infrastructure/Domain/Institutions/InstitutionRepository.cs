using App.Domain.Institutions;
using App.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Domain.Institutions;

public class InstitutionRepository(Context context) : IInstitutionRepository
{
    public async Task AddAsync(Institution institution)
    {
        await context.AddAsync(institution);
    }

    public async Task<Institution> GetByIdAsync(InstitutionId id)
    {
        var institution = await context.Institutions.SingleOrDefaultAsync(i => i.Id == id);

        if (institution is null)
        {
            throw new NotFoundRepositoryException<Institution>(id.Value);
        }

        return institution;
    }

    public async Task<IEnumerable<Institution>> GetEnabledAsync()
    {
        return await context.Institutions.Where(i => i.IsEnabled).ToListAsync();
    }

    public async Task<IEnumerable<Institution>> GetAllAsync()
    {
        return await context.Institutions.ToListAsync();
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
