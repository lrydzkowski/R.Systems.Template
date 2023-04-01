using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Infrastructure.Db.Common.Entities;
using R.Systems.Template.Infrastructure.Db.Common.Mappers;

namespace R.Systems.Template.Infrastructure.Db.Employees.Commands;

internal class CreateEmployeeRepository : ICreateEmployeeRepository
{
    public CreateEmployeeRepository(EmployeeValidator employeeValidator, AppDbContext dbContext)
    {
        EmployeeValidator = employeeValidator;
        DbContext = dbContext;
    }

    private EmployeeValidator EmployeeValidator { get; }
    private AppDbContext DbContext { get; }

    public async Task<Employee> CreateEmployeeAsync(EmployeeToCreate employeeToCreate)
    {
        await EmployeeValidator.VerifyCompanyExistenceAsync(employeeToCreate.CompanyId);

        EmployeeEntityMapper mapper = new();
        EmployeeEntity employeeEntity = mapper.ToEmployeeEntity(employeeToCreate);
        await DbContext.Employees.AddAsync(employeeEntity);
        await DbContext.SaveChangesAsync();

        return mapper.ToEmployee(employeeEntity);
    }
}
