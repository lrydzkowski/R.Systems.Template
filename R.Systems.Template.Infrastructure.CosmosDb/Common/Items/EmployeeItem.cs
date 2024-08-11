namespace R.Systems.Template.Infrastructure.CosmosDb.Common.Items;

internal class EmployeeItem
{
    public const string ContainerName = "employees";

    public string Id { get; set; } = "";

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public long CompanyId { get; set; }
}
