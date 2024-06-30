using MongoDB.Driver;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.DeleteEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;
using R.Systems.Template.Infrastructure.MongoDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.MongoDb.Employees.Commands;

internal class EmployeeRepository : ICreateEmployeeRepository, IUpdateEmployeeRepository, IDeleteEmployeeRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IEmployeeMapper _employeeMapper;

    public EmployeeRepository(AppDbContext appDbContext, IEmployeeMapper employeeMapper)
    {
        _appDbContext = appDbContext;
        _employeeMapper = employeeMapper;
    }

    public string Version { get; } = Versions.V2;

    public async Task<Employee> CreateEmployeeAsync(EmployeeToCreate employeeToCreate)
    {
        EmployeeDocument employeeDocument = _employeeMapper.Map(employeeToCreate);
        await _appDbContext.Employees.InsertOneAsync(employeeDocument);
        Employee createdEmployee = _employeeMapper.Map(employeeDocument);

        return createdEmployee;
    }

    public async Task DeleteEmployeeAsync(long employeeId)
    {
        FilterDefinition<EmployeeDocument>? filter = Builders<EmployeeDocument>.Filter.Eq(x => x.Id, employeeId);
        await _appDbContext.Employees.DeleteOneAsync(filter);
    }

    public async Task<Employee> UpdateEmployeeAsync(EmployeeToUpdate employeeToUpdate)
    {
        FilterDefinition<EmployeeDocument>? filter =
            Builders<EmployeeDocument>.Filter.Where(x => x.Id == employeeToUpdate.EmployeeId);
        EmployeeDocument employeeDocument = _employeeMapper.Map(employeeToUpdate);
        await _appDbContext.Employees.ReplaceOneAsync(filter, employeeDocument);
        Employee employee = _employeeMapper.Map(employeeDocument);

        return employee;
    }
}
