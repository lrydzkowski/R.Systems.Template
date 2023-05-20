using MediatR;

namespace R.Systems.Template.Core.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommand : IRequest
{
    public int EmployeeId { get; init; }
}

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IDeleteEmployeeRepository _deleteEmployeeRepository;

    public DeleteEmployeeCommandHandler(IDeleteEmployeeRepository deleteEmployeeRepository)
    {
        _deleteEmployeeRepository = deleteEmployeeRepository;
    }

    public async Task Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        await _deleteEmployeeRepository.DeleteEmployeeAsync(command.EmployeeId);
    }
}
