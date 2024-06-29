namespace R.Systems.Template.Core.Common.Domain;

public class Employee
{
    public long EmployeeId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public long? CompanyId { get; init; }
}
