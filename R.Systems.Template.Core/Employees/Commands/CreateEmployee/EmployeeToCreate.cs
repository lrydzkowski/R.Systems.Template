namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

public class EmployeeToCreate
{
    public string FirstName { get; init; } = "";
    public string LastName { get; init; } = "";
    public long CompanyId { get; init; }
}
