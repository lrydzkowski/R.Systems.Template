using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Persistence.Db.Common.Entities;
using System.Linq.Expressions;

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
        return await GetEmployeeFromDbAsync();
    }

    public async Task<List<Employee>> GetEmployeesAsync(int companyId)
    {
        return await GetEmployeeFromDbAsync(employeeEntity => employeeEntity.CompanyId == companyId);
    }

    private async Task<List<Employee>> GetEmployeeFromDbAsync(
        Expression<Func<EmployeeEntity, bool>>? wherePredicate = null
    )
    {
        IQueryable<EmployeeEntity> query = DbContext.Employees.AsNoTracking();
        if (wherePredicate != null)
        {
            query = query.Where(wherePredicate);
        }

        return await query
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
