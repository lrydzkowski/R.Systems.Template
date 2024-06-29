namespace R.Systems.Template.Infrastructure.Db.Common.Entities;

internal class CompanyEntity
{
    public long? Id { get; set; }
    public string Name { get; set; } = "";
    public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
}
