namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public class EmployeeToUpdate
{
    public long EmployeeId { get; init; }
    public string FirstName { get; set; } = "";
    public string LastName { get; init; } = "";
    public long CompanyId { get; init; }
}
