namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public class EmployeeToUpdate
{
    public Guid EmployeeId { get; init; }
    public string FirstName { get; set; } = "";
    public string LastName { get; init; } = "";
    public Guid CompanyId { get; init; }
}
