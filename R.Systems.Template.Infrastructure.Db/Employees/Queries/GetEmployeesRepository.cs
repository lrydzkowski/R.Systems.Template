using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;

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
        Expression<Func<Employee, bool>>? wherePredicate = null,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Employee> query = _dbContext.Employees.Select(
                employeeEntity => new Employee
                {
                    EmployeeId = (long)employeeEntity.Id!,
                    FirstName = employeeEntity.FirstName,
                    LastName = employeeEntity.LastName,
                    CompanyId = employeeEntity.CompanyId
                }
            )
            .AsNoTracking();
        if (wherePredicate != null)
        {
            query = query.Where(wherePredicate);
        }

        query = query.Sort(listParameters.Sorting, listParameters.Fields)
            .Filter(listParameters.Filters, listParameters.Fields);
        List<Employee> employees = await query
            .Paginate(listParameters.Pagination)
            .Project(listParameters.Fields)
            .ToListAsync(cancellationToken);
        int count = await query.CountAsync(cancellationToken);

        return new ListInfo<Employee>
        {
            Data = employees,
            Count = count
        };
    }
}
