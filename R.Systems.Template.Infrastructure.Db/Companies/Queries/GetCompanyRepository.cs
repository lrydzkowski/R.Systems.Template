using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Queries.GetCompany;

namespace R.Systems.Template.Infrastructure.Db.Companies.Queries;

internal class GetCompanyRepository : IGetCompanyRepository
{
    private readonly AppDbContext _dbContext;

    public GetCompanyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Company?> GetCompanyAsync(int companyId, CancellationToken cancellationToken)
    {
        return await _dbContext.Companies.AsNoTracking()
            .Where(company => company.Id == companyId)
            .Select(company => new Company { CompanyId = (int)company.Id!, Name = company.Name })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
