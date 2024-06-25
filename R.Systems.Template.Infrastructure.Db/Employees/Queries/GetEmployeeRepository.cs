using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Infrastructure.Db.Common.Entities;

namespace R.Systems.Template.Infrastructure.Db.Employees.Queries;

internal class GetEmployeeRepository : IGetEmployeeRepository
{
    private readonly AppDbContext _dbContext;

    public GetEmployeeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Version { get; } = Versions.V1;

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
        return await _dbContext.Employees.AsNoTracking()
            .Where(wherePredicate)
            .Select(
                employeeEntity => new Employee
                {
                    EmployeeId = (int)employeeEntity.Id!, FirstName = employeeEntity.FirstName,
                    LastName = employeeEntity.LastName, CompanyId = employeeEntity.CompanyId
                }
            )
            .FirstOrDefaultAsync(cancellationToken);
    }
}
