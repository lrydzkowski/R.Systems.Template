namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public class CompanyToUpdate
{
    public long CompanyId { get; init; }
    public string Name { get; set; } = "";
}
