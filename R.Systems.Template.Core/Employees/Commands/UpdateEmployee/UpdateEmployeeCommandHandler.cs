using AutoMapper;
using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommand : IRequest<UpdateEmployeeResult>
{
    public int? EmployeeId { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public int? CompanyId { get; init; }
}

public class UpdateEmployeeResult
{
    public Employee UpdatedEmployee { get; set; } = new();
}

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResult>
{
    public UpdateEmployeeCommandHandler(IMapper mapper, IUpdateEmployeeRepository updateEmployeeRepository)
    {
        Mapper = mapper;
        UpdateEmployeeRepository = updateEmployeeRepository;
    }

    private IMapper Mapper { get; }
    private IUpdateEmployeeRepository UpdateEmployeeRepository { get; }

    public async Task<UpdateEmployeeResult> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        EmployeeToUpdate employeeToUpdate = Mapper.Map<EmployeeToUpdate>(command);
        Employee updatedEmployee = await UpdateEmployeeRepository.UpdateEmployeeAsync(employeeToUpdate);

        return new UpdateEmployeeResult
        {
            UpdatedEmployee = updatedEmployee
        };
    }
}
