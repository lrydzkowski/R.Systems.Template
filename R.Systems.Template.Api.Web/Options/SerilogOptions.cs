namespace R.Systems.Template.Api.Web.Options;

public class SerilogOptions
{
    public const string Position = "Serilog";
    public StorageAccountOptions StorageAccount { get; init; } = new();
}

public class StorageAccountOptions
{
    public string? ConnectionString { get; init; }
    public string? ContainerName { get; init; }
}
