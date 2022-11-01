using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Employees.Commands;

internal class UpdateEmployeeRepository : IUpdateEmployeeRepository
{
    public UpdateEmployeeRepository(EmployeeValidator employeeValidator, IMapper mapper, AppDbContext dbContext)
    {
        EmployeeValidator = employeeValidator;
        Mapper = mapper;
        DbContext = dbContext;
    }

    private EmployeeValidator EmployeeValidator { get; }
    private IMapper Mapper { get; }
    private AppDbContext DbContext { get; }

    public async Task<Result<Employee>> UpdateEmployeeAsync(EmployeeToUpdate employeeToUpdate)
    {
        Result<bool> verifyCompanyExistenceResult =
            await EmployeeValidator.VerifyCompanyExistenceAsync(employeeToUpdate.CompanyId);
        if (verifyCompanyExistenceResult.IsFaulted)
        {
            return verifyCompanyExistenceResult.MapFaulted<Employee>();
        }

        Result<EmployeeEntity> getEmployeeEntityResult = await GetEmployeeEntityAsync(employeeToUpdate.EmployeeId);
        if (getEmployeeEntityResult.IsFaulted)
        {
            return getEmployeeEntityResult.MapFaulted<Employee>();
        }

        EmployeeEntity employeeEntity = getEmployeeEntityResult.Value!;
        employeeEntity.FirstName = employeeToUpdate.FirstName;
        employeeEntity.LastName = employeeToUpdate.LastName;
        employeeEntity.CompanyId = employeeToUpdate.CompanyId;

        await DbContext.SaveChangesAsync();

        return Mapper.Map<Employee>(employeeEntity);
    }

    private async Task<Result<EmployeeEntity>> GetEmployeeEntityAsync(int employeeId)
    {
        EmployeeEntity? employeeEntity = await DbContext.Employees.Where(x => x.Id == employeeId)
            .FirstOrDefaultAsync();
        if (employeeEntity == null)
        {
            ValidationException validationException = new(
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

            return new Result<EmployeeEntity>(validationException);
        }

        return employeeEntity;
    }
}
