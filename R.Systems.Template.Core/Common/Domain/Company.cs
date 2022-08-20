namespace R.Systems.Template.Core.Common.Domain;

public class Company
{
    public int CompanyId { get; set; }

    public string Name { get; set; } = "";

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
