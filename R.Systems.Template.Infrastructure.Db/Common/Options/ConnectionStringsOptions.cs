namespace R.Systems.Template.Infrastructure.Db.Common.Options;

internal class ConnectionStringsOptions
{
    public const string Position = "ConnectionStrings";

    public string? AppSqlServerDb { get; set; }

    public string? AppPostgresDb { get; set; }

    public string? StorageAccount { get; init; }
}
