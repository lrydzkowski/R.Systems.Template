using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<CreateEmployeeResult>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? CompanyId { get; set; }
}

public class CreateEmployeeResult
{
    public Employee CreatedEmployee { get; init; } = new();
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResult>
{
    private readonly ICreateEmployeeRepository _createEmployeeRepository;

    public CreateEmployeeCommandHandler(ICreateEmployeeRepository createEmployeeRepository)
    {
        _createEmployeeRepository = createEmployeeRepository;
    }

    public async Task<CreateEmployeeResult> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        CreateEmployeeCommandMapper mapper = new();
        EmployeeToCreate employeeToCreate = mapper.ToEmployeeToCreate(command);
        Employee createdEmployee = await _createEmployeeRepository.CreateEmployeeAsync(employeeToCreate);
        return new CreateEmployeeResult
        {
            CreatedEmployee = createdEmployee
        };
    }
}
