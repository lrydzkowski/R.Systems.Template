using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommand : IContextRequest, IRequest<UpdateEmployeeResult>
{
    public int EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? CompanyId { get; set; }
    public ApplicationContext AppContext { get; set; } = new();
}

public class UpdateEmployeeResult
{
    public Employee UpdatedEmployee { get; set; } = new();
}

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResult>
{
    private readonly IVersionedRepositoryFactory<IUpdateEmployeeRepository> _repositoryFactory;

    public UpdateEmployeeCommandHandler(IVersionedRepositoryFactory<IUpdateEmployeeRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<UpdateEmployeeResult> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        IUpdateEmployeeRepository repository = _repositoryFactory.GetRepository(command.AppContext);
        UpdateEmployeeCommandMapper mapper = new();
        EmployeeToUpdate employeeToUpdate = mapper.ToEmployeeToUpdate(command);
        Employee updatedEmployee = await repository.UpdateEmployeeAsync(employeeToUpdate);
        return new UpdateEmployeeResult
        {
            UpdatedEmployee = updatedEmployee
        };
    }
}
