namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public class EmployeeToUpdate
{
    public int EmployeeId { get; init; }
    public string FirstName { get; set; } = "";
    public string LastName { get; init; } = "";
    public int CompanyId { get; init; }
}
