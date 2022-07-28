namespace R.Systems.Template.Core.Common.DataTransferObjects;

public class CompanyDto
{
    public int CompanyId { get; set; }

    public string Name { get; set; } = "";

    public ICollection<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
}
