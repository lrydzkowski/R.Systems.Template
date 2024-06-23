namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

public class EmployeeToCreate
{
    public string FirstName { get; init; } = "";
    public string LastName { get; init; } = "";
    public int CompanyId { get; init; }
}
