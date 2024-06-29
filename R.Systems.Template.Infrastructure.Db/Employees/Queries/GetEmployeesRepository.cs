using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Infrastructure.Db.Common.Entities;

namespace R.Systems.Template.Infrastructure.Db.Employees.Queries;

internal class GetEmployeesRepository : IGetEmployeesRepository
{
    private readonly AppDbContext _dbContext;

    public GetEmployeesRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Version { get; } = Versions.V1;

    public async Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        return await GetEmployeeFromDbAsync(listParameters, cancellationToken: cancellationToken);
    }

    public async Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        long companyId,
        CancellationToken cancellationToken
    )
    {
        return await GetEmployeeFromDbAsync(
            listParameters,
            employeeEntity => employeeEntity.CompanyId == companyId,
            cancellationToken
        );
    }

    private async Task<ListInfo<Employee>> GetEmployeeFromDbAsync(
        ListParameters listParameters,
        Expression<Func<EmployeeEntity, bool>>? wherePredicate = null,
        CancellationToken cancellationToken = default
    )
    {
        List<string> fieldsAvailableToSort = new()
        {
            "id",
            "firstName",
            "lastName"
        };
        List<string> fieldsAvailableToFilter = new()
        {
            "firstName",
            "lastName"
        };
        IQueryable<EmployeeEntity> query = _dbContext.Employees.AsNoTracking();
        if (wherePredicate != null)
        {
            query = query.Where(wherePredicate);
        }

        List<Employee> employees = await query.Sort(fieldsAvailableToSort, listParameters.Sorting, "id")
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Paginate(listParameters.Pagination)
            .Select(
                employeeEntity => new Employee
                {
                    EmployeeId = (long)employeeEntity.Id!, FirstName = employeeEntity.FirstName,
                    LastName = employeeEntity.LastName, CompanyId = employeeEntity.CompanyId
                }
            )
            .ToListAsync(cancellationToken);
        int count = await query.Sort(fieldsAvailableToSort, listParameters.Sorting, "id")
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Select(employeeEntity => (long)employeeEntity.Id!)
            .CountAsync(cancellationToken);
        return new ListInfo<Employee>
        {
            Data = employees,
            Count = count
        };
    }
}
