using AutoMapper;
using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommand : IRequest<Result<Employee>>
{
    public int? EmployeeId { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public int? CompanyId { get; init; }
}

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result<Employee>>
{
    public UpdateEmployeeCommandHandler(IMapper mapper, IUpdateEmployeeRepository updateEmployeeRepository)
    {
        Mapper = mapper;
        UpdateEmployeeRepository = updateEmployeeRepository;
    }

    private IMapper Mapper { get; }
    private IUpdateEmployeeRepository UpdateEmployeeRepository { get; }

    public async Task<Result<Employee>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        EmployeeToUpdate employeeToUpdate = Mapper.Map<EmployeeToUpdate>(command);

        return await UpdateEmployeeRepository.UpdateEmployeeAsync(employeeToUpdate);
    }
}
