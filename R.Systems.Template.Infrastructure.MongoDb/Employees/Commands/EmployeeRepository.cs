using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.DeleteEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

namespace R.Systems.Template.Infrastructure.MongoDb.Employees.Commands;

internal class EmployeeRepository : ICreateEmployeeRepository, IUpdateEmployeeRepository, IDeleteEmployeeRepository
{
    public string Version { get; } = Versions.V2;

    public Task<Employee> CreateEmployeeAsync(EmployeeToCreate employeeToCreate)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmployeeAsync(long employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<Employee> UpdateEmployeeAsync(EmployeeToUpdate employeeToUpdate)
    {
        throw new NotImplementedException();
    }
}
