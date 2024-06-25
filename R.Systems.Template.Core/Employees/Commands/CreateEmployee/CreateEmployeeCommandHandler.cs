using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IContextRequest, IRequest<CreateEmployeeResult>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? CompanyId { get; set; }
    public ApplicationContext AppContext { get; set; } = new();
}

public class CreateEmployeeResult
{
    public Employee CreatedEmployee { get; init; } = new();
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResult>
{
    private readonly IVersionedRepositoryFactory<ICreateEmployeeRepository> _repositoryFactory;

    public CreateEmployeeCommandHandler(IVersionedRepositoryFactory<ICreateEmployeeRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<CreateEmployeeResult> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        ICreateEmployeeRepository repository = _repositoryFactory.GetRepository(command.AppContext);
        CreateEmployeeCommandMapper mapper = new();
        EmployeeToCreate employeeToCreate = mapper.ToEmployeeToCreate(command);
        Employee createdEmployee = await repository.CreateEmployeeAsync(employeeToCreate);
        return new CreateEmployeeResult
        {
            CreatedEmployee = createdEmployee
        };
    }
}
