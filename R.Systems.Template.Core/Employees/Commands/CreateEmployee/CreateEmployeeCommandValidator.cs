using FluentValidation;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithName(nameof(CreateEmployeeCommand.FirstName));
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithName(nameof(CreateEmployeeCommand.LastName));
        RuleFor(c => c.CompanyId).NotEmpty().WithName(nameof(CreateEmployeeCommand.CompanyId));
    }
}
