using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Queries.GetCompany;

namespace R.Systems.Template.Persistence.Db.Companies.Queries;

internal class GetCompanyRepository : IGetCompanyRepository
{
    public GetCompanyRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<Company?> GetCompanyAsync(int companyId)
    {
        return await DbContext.Companies.AsNoTracking()
            .Where(company => company.Id == companyId)
            .Select(
                company => new Company
                {
                    CompanyId = (int)company.Id!,
                    Name = company.Name,
                    Employees = company.Employees.Select(
                            employee => new Employee
                            {
                                EmployeeId = (int)employee.Id!,
                                FirstName = employee.FirstName,
                                LastName = employee.LastName
                            }
                        )
                        .ToList()
                }
            )
            .FirstOrDefaultAsync();
    }
}
