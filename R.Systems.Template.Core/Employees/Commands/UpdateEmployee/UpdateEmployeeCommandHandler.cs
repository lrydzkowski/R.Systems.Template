using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommand : IRequest<UpdateEmployeeResult>
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? CompanyId { get; set; }
}

public class UpdateEmployeeResult
{
    public Employee UpdatedEmployee { get; set; } = new();
}

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResult>
{
    public UpdateEmployeeCommandHandler(IUpdateEmployeeRepository updateEmployeeRepository)
    {
        UpdateEmployeeRepository = updateEmployeeRepository;
    }

    private IUpdateEmployeeRepository UpdateEmployeeRepository { get; }

    public async Task<UpdateEmployeeResult> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        UpdateEmployeeCommandMapper mapper = new();
        EmployeeToUpdate employeeToUpdate = mapper.ToEmployeeToUpdate(command);
        Employee updatedEmployee = await UpdateEmployeeRepository.UpdateEmployeeAsync(employeeToUpdate);

        return new UpdateEmployeeResult
        {
            UpdatedEmployee = updatedEmployee
        };
    }
}
