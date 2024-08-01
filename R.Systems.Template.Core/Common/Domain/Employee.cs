namespace R.Systems.Template.Core.Common.Domain;

public class Employee
{
    public Guid EmployeeId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public Guid? CompanyId { get; init; }
}
