using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Infrastructure.Db.Common.Entities;
using System.Linq.Expressions;

namespace R.Systems.Template.Infrastructure.Db.Employees.Queries;

internal class GetEmployeesRepository : IGetEmployeesRepository
{
    public GetEmployeesRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<List<Employee>> GetEmployeesAsync(ListParameters listParameters)
    {
        return await GetEmployeeFromDbAsync(listParameters);
    }

    public async Task<List<Employee>> GetEmployeesAsync(ListParameters listParameters, int companyId)
    {
        return await GetEmployeeFromDbAsync(listParameters, employeeEntity => employeeEntity.CompanyId == companyId);
    }

    private async Task<List<Employee>> GetEmployeeFromDbAsync(
        ListParameters listParameters,
        Expression<Func<EmployeeEntity, bool>>? wherePredicate = null
    )
    {
        List<string> fieldsAvailableToSort = new() { "id", "firstName", "lastName" };
        List<string> fieldsAvailableToFilter = new() { "firstName", "lastName" };

        IQueryable<EmployeeEntity> query = DbContext.Employees.AsNoTracking();
        if (wherePredicate != null)
        {
            query = query.Where(wherePredicate);
        }

        List<Employee> employees = await query
            .Sort(fieldsAvailableToSort, listParameters.Sorting, "id")
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Paginate(listParameters.Pagination)
            .Select(
                employeeEntity => new Employee
                {
                    EmployeeId = (int)employeeEntity.Id!,
                    FirstName = employeeEntity.FirstName,
                    LastName = employeeEntity.LastName,
                    CompanyId = employeeEntity.CompanyId
                }
            )
            .ToListAsync();

        return employees;
    }
}
