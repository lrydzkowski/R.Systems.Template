using FluentValidation;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

internal class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(c => c.CompanyId).NotEmpty();
    }
}
