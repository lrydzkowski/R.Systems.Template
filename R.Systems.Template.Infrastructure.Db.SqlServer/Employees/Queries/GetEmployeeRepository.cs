using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Template.Infrastructure.Db.SqlServer.Employees.Queries;

internal class GetEmployeeRepository : IGetEmployeeRepository
{
    public GetEmployeeRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<Employee?> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken)
    {
        return await GetEmployeeFromDbAsync(employeeEntity => employeeEntity.Id == employeeId, cancellationToken);
    }

    public async Task<Employee?> GetEmployeeAsync(int companyId, int employeeId, CancellationToken cancellationToken)
    {
        return await GetEmployeeFromDbAsync(
            employeeEntity => employeeEntity.CompanyId == companyId && employeeEntity.Id == employeeId,
            cancellationToken
        );
    }

    private async Task<Employee?> GetEmployeeFromDbAsync(
        Expression<Func<EmployeeEntity, bool>> wherePredicate,
        CancellationToken cancellationToken
    )
    {
        return await DbContext.Employees.AsNoTracking()
            .Where(wherePredicate)
            .Select(
                employeeEntity => new Employee
                {
                    EmployeeId = (int)employeeEntity.Id!,
                    FirstName = employeeEntity.FirstName,
                    LastName = employeeEntity.LastName,
                    CompanyId = employeeEntity.CompanyId
                }
            )
            .FirstOrDefaultAsync(cancellationToken);
    }
}
