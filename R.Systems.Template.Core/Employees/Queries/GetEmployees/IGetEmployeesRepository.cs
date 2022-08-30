using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployees;

public interface IGetEmployeesRepository
{
    Task<List<Employee>> GetEmployeesAsync();
}
