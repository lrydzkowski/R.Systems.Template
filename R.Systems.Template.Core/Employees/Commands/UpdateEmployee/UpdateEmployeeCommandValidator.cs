using FluentValidation;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(c => c.EmployeeId).NotEmpty().WithName(nameof(UpdateEmployeeCommand.EmployeeId));
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithName(nameof(UpdateEmployeeCommand.FirstName));
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithName(nameof(UpdateEmployeeCommand.LastName));
        RuleFor(c => c.CompanyId).NotEmpty().WithName(nameof(UpdateEmployeeCommand.CompanyId));
    }
}
