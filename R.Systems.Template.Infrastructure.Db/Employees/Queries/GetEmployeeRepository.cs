using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Infrastructure.Db.Common.Entities;

namespace R.Systems.Template.Infrastructure.Db.Employees.Queries;

internal class GetEmployeeRepository : IGetEmployeeRepository
{
    public GetEmployeeRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<Employee?> GetEmployeeAsync(int employeeId)
    {
        return await GetEmployeeFromDbAsync(employeeEntity => employeeEntity.Id == employeeId);
    }

    public async Task<Employee?> GetEmployeeAsync(int companyId, int employeeId)
    {
        return await GetEmployeeFromDbAsync(
            employeeEntity => employeeEntity.CompanyId == companyId && employeeEntity.Id == employeeId
        );
    }

    private async Task<Employee?> GetEmployeeFromDbAsync(Expression<Func<EmployeeEntity, bool>> wherePredicate)
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
            .FirstOrDefaultAsync();
    }
}
