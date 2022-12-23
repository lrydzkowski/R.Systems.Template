namespace R.Systems.Template.Infrastructure.Db.Common.Entities;

internal class EmployeeEntity
{
    public int? Id { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public int? CompanyId { get; set; }
    public CompanyEntity? Company { get; set; }
}
