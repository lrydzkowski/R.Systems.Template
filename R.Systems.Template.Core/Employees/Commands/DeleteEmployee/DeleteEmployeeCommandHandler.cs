using MediatR;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommand : IContextRequest, IRequest
{
    public int EmployeeId { get; init; }
    public ApplicationContext AppContext { get; set; } = new();
}

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IVersionedRepositoryFactory<IDeleteEmployeeRepository> _repositoryFactory;

    public DeleteEmployeeCommandHandler(IVersionedRepositoryFactory<IDeleteEmployeeRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        IDeleteEmployeeRepository repository = _repositoryFactory.GetRepository(command.AppContext);
        await repository.DeleteEmployeeAsync(command.EmployeeId);
    }
}
