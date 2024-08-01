namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public class CompanyToUpdate
{
    public Guid CompanyId { get; init; }
    public string Name { get; set; } = "";
}
