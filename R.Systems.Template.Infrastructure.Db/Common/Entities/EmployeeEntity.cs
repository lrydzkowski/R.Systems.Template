namespace R.Systems.Template.Infrastructure.Db.Common.Entities;

internal class EmployeeEntity
{
    public long? Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public long? CompanyId { get; set; }
    public CompanyEntity? Company { get; set; }
}
