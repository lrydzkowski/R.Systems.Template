using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;

namespace R.Systems.Template.Persistence.Db.Companies.Queries;

internal class GetCompaniesRepository : IGetCompaniesRepository
{
    public GetCompaniesRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<List<Company>> GetCompaniesAsync(ListParameters listParameters)
    {
        List<string> fieldsAvailableToSort = new() { "id", "name" };
        List<string> fieldsAvailableToFilter = new() { "name" };

        return await DbContext.Companies.AsNoTracking()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, "id")
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Paginate(listParameters.Pagination)
            .Select(
                companyEntity => new Company
                {
                    CompanyId = (int)companyEntity.Id!,
                    Name = companyEntity.Name,
                    Employees = companyEntity.Employees.Select(
                            employeeEntity => new Employee
                            {
                                EmployeeId = (int)employeeEntity.Id!,
                                FirstName = employeeEntity.FirstName,
                                LastName = employeeEntity.LastName
                            }
                        )
                        .ToList()
                }
            )
            .ToListAsync();
    }
}
