using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployees;

public interface IGetEmployeesRepository
{
    Task<List<Employee>> GetEmployeesAsync(ListParameters listParameters);

    Task<List<Employee>> GetEmployeesAsync(ListParameters listParameters, int companyId);
}
