using FluentValidation;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

internal class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(c => c.EmployeeId).NotEmpty();
        Transform(c => c.FirstName, x => x?.Trim()).NotEmpty().MaximumLength(100);
        Transform(c => c.LastName, x => x?.Trim()).NotEmpty().MaximumLength(100);
        RuleFor(c => c.CompanyId).NotEmpty();
    }
}
