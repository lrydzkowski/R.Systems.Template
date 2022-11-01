using AutoMapper;
using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<Result<Employee>>
{
    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public int? CompanyId { get; init; }
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<Employee>>
{
    public CreateEmployeeCommandHandler(IMapper mapper, ICreateEmployeeRepository createEmployeeRepository)
    {
        Mapper = mapper;
        CreateEmployeeRepository = createEmployeeRepository;
    }

    private IMapper Mapper { get; }
    private ICreateEmployeeRepository CreateEmployeeRepository { get; }

    public async Task<Result<Employee>> Handle(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken
    )
    {
        EmployeeToCreate employeeToCreate = Mapper.Map<EmployeeToCreate>(command);

        return await CreateEmployeeRepository.CreateEmployeeAsync(employeeToCreate);
    }
}
