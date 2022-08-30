using FluentValidation;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

internal class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        Transform(c => c.FirstName, x => x?.Trim()).NotEmpty().MaximumLength(100);
        Transform(c => c.LastName, x => x?.Trim()).NotEmpty().MaximumLength(100);
        RuleFor(c => c.CompanyId).NotEmpty();
    }
}
