using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployee;

public interface IGetEmployeeRepository
{
    Task<Employee?> GetEmployeeAsync(int employeeId);

    Task<Employee?> GetEmployeeAsync(int companyId, int employeeId);
}
