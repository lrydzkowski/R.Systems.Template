using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;

namespace R.Systems.Template.Persistence.Db.Employees.Queries;

internal class GetEmployeesRepository : IGetEmployeesRepository
{
    public GetEmployeesRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<List<Employee>> GetEmployeesAsync()
    {
        return await DbContext.Employees.AsNoTracking()
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
    }
}
