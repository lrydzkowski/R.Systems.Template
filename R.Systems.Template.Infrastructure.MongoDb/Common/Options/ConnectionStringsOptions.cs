namespace R.Systems.Template.Infrastructure.MongoDb.Common.Options;

internal class ConnectionStringsOptions
{
    public const string Position = "ConnectionStrings";

    public string? MongoDb { get; set; }
}
