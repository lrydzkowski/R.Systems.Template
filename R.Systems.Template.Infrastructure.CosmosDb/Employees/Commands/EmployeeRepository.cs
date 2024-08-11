using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.DeleteEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Items;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.CosmosDb.Employees.Commands;

internal class EmployeeRepository : ICreateEmployeeRepository, IUpdateEmployeeRepository, IDeleteEmployeeRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IEmployeeMapper _employeeMapper;
    private readonly IGetEmployeeRepository _getEmployeeRepository;

    public EmployeeRepository(
        AppDbContext appDbContext,
        IEmployeeMapper employeeMapper,
        IGetEmployeeRepository getEmployeeRepository
    )
    {
        _appDbContext = appDbContext;
        _employeeMapper = employeeMapper;
        _getEmployeeRepository = getEmployeeRepository;
    }

    public string Version { get; } = Versions.V4;

    public async Task<Employee> CreateEmployeeAsync(EmployeeToCreate employeeToCreate)
    {
        EmployeeItem employeeItem = _employeeMapper.Map(employeeToCreate);
        EmployeeItem createdEmployeeItem = await _appDbContext.EmployeesContainers.CreateItemAsync(
            employeeItem,
            new PartitionKey(employeeItem.CompanyId.ToString())
        );

        Employee createdEmployee = _employeeMapper.Map(createdEmployeeItem);

        return createdEmployee;
    }

    public async Task DeleteEmployeeAsync(long employeeId)
    {
        Employee? employee = await _getEmployeeRepository.GetEmployeeAsync(employeeId, CancellationToken.None);
        if (employee is null)
        {
            return;
        }

        await _appDbContext.EmployeesContainers.DeleteItemAsync<EmployeeItem>(
            employeeId.ToString(),
            new PartitionKey(employee.CompanyId.ToString())
        );
    }

    public async Task<Employee> UpdateEmployeeAsync(EmployeeToUpdate employeeToUpdate)
    {
        EmployeeItem employeeItem = _employeeMapper.Map(employeeToUpdate);
        EmployeeItem updatedEmployeeItem = await _appDbContext.EmployeesContainers.UpsertItemAsync(
            employeeItem,
            new PartitionKey(employeeItem.CompanyId.ToString())
        );

        Employee updatedEmployee = _employeeMapper.Map(updatedEmployeeItem);

        return updatedEmployee;
    }
}
