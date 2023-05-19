using FluentValidation;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployee;

public class GetEmployeeQueryValidator : AbstractValidator<GetEmployeeQuery>
{
    public GetEmployeeQueryValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty().WithName(nameof(GetEmployeeQuery.EmployeeId));
    }
}
