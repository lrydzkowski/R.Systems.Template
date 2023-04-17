using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Queries.GetCompany;

namespace R.Systems.Template.Infrastructure.Db.SqlServer.Companies.Queries;

internal class GetCompanyRepository : IGetCompanyRepository
{
    public GetCompanyRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<Company?> GetCompanyAsync(int companyId, CancellationToken cancellationToken)
    {
        return await DbContext.Companies.AsNoTracking()
            .Where(company => company.Id == companyId)
            .Select(
                company => new Company
                {
                    CompanyId = (int)company.Id!,
                    Name = company.Name
                }
            )
            .FirstOrDefaultAsync(cancellationToken);
    }
}
