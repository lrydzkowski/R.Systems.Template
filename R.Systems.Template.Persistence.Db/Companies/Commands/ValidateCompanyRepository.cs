using Microsoft.EntityFrameworkCore;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class ValidateCompanyRepository
{
    public ValidateCompanyRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<bool> CompanyNameExists(string name)
    {
        int? companyId = await DbContext.Companies.AsNoTracking()
            .Where(companyEntity => companyEntity.Name == name)
            .Select(companyEntity => companyEntity.Id)
            .FirstOrDefaultAsync();

        return companyId != null;
    }
}
