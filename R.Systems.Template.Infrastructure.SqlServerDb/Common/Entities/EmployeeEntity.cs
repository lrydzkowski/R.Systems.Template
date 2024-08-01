namespace R.Systems.Template.Infrastructure.SqlServerDb.Common.Entities;

internal class EmployeeEntity
{
    public Guid? Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public Guid? CompanyId { get; set; }
    public CompanyEntity? Company { get; set; }
}
