namespace R.Systems.Template.Core.Common.Domain;

public class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public int? CompanyId { get; init; }
}
