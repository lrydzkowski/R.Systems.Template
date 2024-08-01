using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.DeleteEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;
using EmployeeEntityMapper = R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Mappers.EmployeeEntityMapper;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Employees.Commands;

internal class EmployeeRepository : ICreateEmployeeRepository, IUpdateEmployeeRepository, IDeleteEmployeeRepository
{
    private readonly AppDbContext _dbContext;

    private readonly EmployeeValidator _employeeValidator;

    public EmployeeRepository(EmployeeValidator employeeValidator, AppDbContext dbContext)
    {
        _employeeValidator = employeeValidator;
        _dbContext = dbContext;
    }

    public string Version { get; } = Versions.V1;

    public async Task<Employee> CreateEmployeeAsync(EmployeeToCreate employeeToCreate)
    {
        await _employeeValidator.VerifyCompanyExistenceAsync(employeeToCreate.CompanyId);
        EmployeeEntityMapper mapper = new();
        EmployeeEntity employeeEntity = mapper.ToEmployeeEntity(employeeToCreate);
        employeeEntity.Id = Guid.NewGuid();
        await _dbContext.Employees.AddAsync(employeeEntity);
        await _dbContext.SaveChangesAsync();
        return mapper.ToEmployee(employeeEntity);
    }

    public async Task DeleteEmployeeAsync(Guid employeeId)
    {
        EmployeeEntity employeeEntity = await GetEmployeeEntityAsync(employeeId);
        _dbContext.Employees.Remove(employeeEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Employee> UpdateEmployeeAsync(EmployeeToUpdate employeeToUpdate)
    {
        await _employeeValidator.VerifyCompanyExistenceAsync(employeeToUpdate.CompanyId);
        EmployeeEntity employeeEntity = await GetEmployeeEntityAsync(employeeToUpdate.EmployeeId);
        employeeEntity.FirstName = employeeToUpdate.FirstName;
        employeeEntity.LastName = employeeToUpdate.LastName;
        employeeEntity.CompanyId = employeeToUpdate.CompanyId;
        await _dbContext.SaveChangesAsync();
        EmployeeEntityMapper mapper = new();
        Employee employee = mapper.ToEmployee(employeeEntity);
        return employee;
    }

    private async Task<EmployeeEntity> GetEmployeeEntityAsync(Guid employeeId)
    {
        EmployeeEntity? employeeEntity =
            await _dbContext.Employees.Where(x => x.Id == employeeId).FirstOrDefaultAsync();
        if (employeeEntity == null)
        {
            throw new ValidationException(
                new List<ValidationFailure>
                {
                    new()
                    {
                        PropertyName = "Employee",
                        ErrorMessage = $"Employee with the given id doesn't exist ('{employeeId}').",
                        AttemptedValue = employeeId, ErrorCode = "NotExist"
                    }
                }
            );
        }

        return employeeEntity;
    }
}
