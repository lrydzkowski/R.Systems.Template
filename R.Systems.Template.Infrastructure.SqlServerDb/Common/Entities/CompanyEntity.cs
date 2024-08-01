namespace R.Systems.Template.Infrastructure.SqlServerDb.Common.Entities;

internal class CompanyEntity
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = "";
    public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
}
