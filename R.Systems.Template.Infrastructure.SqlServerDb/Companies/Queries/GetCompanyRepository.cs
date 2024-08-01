using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Queries.GetCompany;

namespace R.Systems.Template.Infrastructure.SqlServerDb.Companies.Queries;

internal class GetCompanyRepository : IGetCompanyRepository
{
    private readonly AppDbContext _dbContext;

    public GetCompanyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Version { get; } = Versions.V3;

    public async Task<Company?> GetCompanyAsync(Guid companyId, CancellationToken cancellationToken)
    {
        return await _dbContext.Companies.AsNoTracking()
            .Where(company => company.Id == companyId)
            .Select(company => new Company { CompanyId = (Guid)company.Id!, Name = company.Name })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
