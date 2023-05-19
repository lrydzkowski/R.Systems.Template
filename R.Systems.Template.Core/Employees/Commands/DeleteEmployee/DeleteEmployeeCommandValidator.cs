using FluentValidation;

namespace R.Systems.Template.Core.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty().WithName(nameof(DeleteEmployeeCommand.EmployeeId));
    }
}
