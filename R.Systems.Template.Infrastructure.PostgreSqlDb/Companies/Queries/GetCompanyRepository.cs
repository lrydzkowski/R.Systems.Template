using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Queries.GetCompany;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Companies.Queries;

internal class GetCompanyRepository : IGetCompanyRepository
{
    private readonly AppDbContext _dbContext;

    public GetCompanyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Version { get; } = Versions.V1;

    public async Task<Company?> GetCompanyAsync(long companyId, CancellationToken cancellationToken)
    {
        return await _dbContext.Companies.AsNoTracking()
            .Where(company => company.Id == companyId)
            .Select(company => new Company { CompanyId = (long)company.Id!, Name = company.Name })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
