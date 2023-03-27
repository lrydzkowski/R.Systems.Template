namespace R.Systems.Template.Infrastructure.Db.Postgres.Common.Entities;

internal class CompanyEntity
{
    public int? Id { get; set; }

    public string Name { get; set; } = "";

    public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
}
