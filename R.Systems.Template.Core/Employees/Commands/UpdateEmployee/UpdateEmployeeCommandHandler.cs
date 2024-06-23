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
    private readonly IUpdateEmployeeRepository _updateEmployeeRepository;

    public UpdateEmployeeCommandHandler(IUpdateEmployeeRepository updateEmployeeRepository)
    {
        _updateEmployeeRepository = updateEmployeeRepository;
    }

    public async Task<UpdateEmployeeResult> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        UpdateEmployeeCommandMapper mapper = new();
        EmployeeToUpdate employeeToUpdate = mapper.ToEmployeeToUpdate(command);
        Employee updatedEmployee = await _updateEmployeeRepository.UpdateEmployeeAsync(employeeToUpdate);
        return new UpdateEmployeeResult
        {
            UpdatedEmployee = updatedEmployee
        };
    }
}
