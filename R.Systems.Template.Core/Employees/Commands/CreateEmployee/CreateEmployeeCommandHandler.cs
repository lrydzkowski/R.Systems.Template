using AutoMapper;
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
    public CreateEmployeeCommandHandler(IMapper mapper, ICreateEmployeeRepository createEmployeeRepository)
    {
        Mapper = mapper;
        CreateEmployeeRepository = createEmployeeRepository;
    }

    private IMapper Mapper { get; }
    private ICreateEmployeeRepository CreateEmployeeRepository { get; }

    public async Task<CreateEmployeeResult> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        EmployeeToCreate employeeToCreate = Mapper.Map<EmployeeToCreate>(command);
        Employee createdEmployee = await CreateEmployeeRepository.CreateEmployeeAsync(employeeToCreate);

        return new CreateEmployeeResult
        {
            CreatedEmployee = createdEmployee
        };
    }
}
