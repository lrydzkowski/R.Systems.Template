using AutoMapper;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Infrastructure.Db.Common.Entities;

namespace R.Systems.Template.Infrastructure.Db.Employees.Commands;

internal class CreateEmployeeRepository : ICreateEmployeeRepository
{
    public CreateEmployeeRepository(EmployeeValidator employeeValidator, IMapper mapper, AppDbContext dbContext)
    {
        EmployeeValidator = employeeValidator;
        Mapper = mapper;
        DbContext = dbContext;
    }

    private EmployeeValidator EmployeeValidator { get; }
    private IMapper Mapper { get; }
    private AppDbContext DbContext { get; }

    public async Task<Employee> CreateEmployeeAsync(EmployeeToCreate employeeToCreate)
    {
        await EmployeeValidator.VerifyCompanyExistenceAsync(employeeToCreate.CompanyId);

        EmployeeEntity employeeEntity = Mapper.Map<EmployeeEntity>(employeeToCreate);
        await DbContext.Employees.AddAsync(employeeEntity);
        await DbContext.SaveChangesAsync();

        return Mapper.Map<Employee>(employeeEntity);
    }
}
