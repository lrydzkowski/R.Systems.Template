using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.DeleteEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.CosmosDb.Employees.Commands;

internal class EmployeeRepository : ICreateEmployeeRepository, IUpdateEmployeeRepository, IDeleteEmployeeRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly IEmployeeMapper _employeeMapper;

    public EmployeeRepository(CosmosClient cosmosClient, IEmployeeMapper employeeMapper)
    {
        _cosmosClient = cosmosClient;
        _employeeMapper = employeeMapper;
    }

    public string Version { get; } = Versions.V4;

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
