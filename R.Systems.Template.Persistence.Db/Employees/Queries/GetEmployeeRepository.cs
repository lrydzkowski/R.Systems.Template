using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;

namespace R.Systems.Template.Persistence.Db.Employees.Queries;

internal class GetEmployeeRepository : IGetEmployeeRepository
{
    public GetEmployeeRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<Employee?> GetEmployeeAsync(int employeeId)
    {
        return await DbContext.Employees.AsNoTracking()
            .Where(employeeEntity => employeeEntity.Id == employeeId)
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
