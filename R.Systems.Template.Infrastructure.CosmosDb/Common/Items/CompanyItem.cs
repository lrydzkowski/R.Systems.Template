namespace R.Systems.Template.Infrastructure.CosmosDb.Common.Items;

internal class CompanyItem
{
    public const string ContainerName = "companies";

    public string Id { get; set; } = "";

    public string Name { get; set; } = "";
}
