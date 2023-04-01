using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Infrastructure.Db.Common.Entities;
using R.Systems.Template.Infrastructure.Db.Common.Mappers;

namespace R.Systems.Template.Infrastructure.Db.Employees.Commands;

internal class UpdateEmployeeRepository : IUpdateEmployeeRepository
{
    public UpdateEmployeeRepository(EmployeeValidator employeeValidator, AppDbContext dbContext)
    {
        EmployeeValidator = employeeValidator;
        DbContext = dbContext;
    }

    private EmployeeValidator EmployeeValidator { get; }
    private AppDbContext DbContext { get; }

    public async Task<Employee> UpdateEmployeeAsync(EmployeeToUpdate employeeToUpdate)
    {
        EmployeeEntityMapper mapper = new();
        await EmployeeValidator.VerifyCompanyExistenceAsync(employeeToUpdate.CompanyId);

        EmployeeEntity employeeEntity = await GetEmployeeEntityAsync(employeeToUpdate.EmployeeId);
        employeeEntity.FirstName = employeeToUpdate.FirstName;
        employeeEntity.LastName = employeeToUpdate.LastName;
        employeeEntity.CompanyId = employeeToUpdate.CompanyId;

        await DbContext.SaveChangesAsync();

        return mapper.ToEmployee(employeeEntity);
    }

    private async Task<EmployeeEntity> GetEmployeeEntityAsync(int employeeId)
    {
        EmployeeEntity? employeeEntity = await DbContext.Employees.Where(x => x.Id == employeeId)
            .FirstOrDefaultAsync();
        if (employeeEntity == null)
        {
            throw new ValidationException(
                new List<ValidationFailure>
                {
                    new()
                    {
                        PropertyName = "Employee",
                        ErrorMessage = $"Employee with the given id doesn't exist ('{employeeId}').",
                        AttemptedValue = employeeId,
                        ErrorCode = "NotExist"
                    }
                }
            );
        }

        return employeeEntity;
    }
}
