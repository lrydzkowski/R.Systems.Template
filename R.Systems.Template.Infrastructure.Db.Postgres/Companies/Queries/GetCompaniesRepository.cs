using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;

namespace R.Systems.Template.Infrastructure.Db.Postgres.Companies.Queries;

internal class GetCompaniesRepository : IGetCompaniesRepository
{
    public GetCompaniesRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<ListInfo<Company>> GetCompaniesAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        List<string> fieldsAvailableToSort = new() { "id", "name" };
        List<string> fieldsAvailableToFilter = new() { "name" };

        List<Company> companies = await DbContext.Companies.AsNoTracking()
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
                        .OrderBy(employee => employee.EmployeeId)
                        .ToList()
                }
            )
            .ToListAsync(cancellationToken);
        int count = await DbContext.Companies.AsNoTracking()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, "id")
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Select(
                companyEntity => (int)companyEntity.Id!
            )
            .CountAsync(cancellationToken);

        return new ListInfo<Company>()
        {
            Data = companies,
            Count = count
        };
    }
}
