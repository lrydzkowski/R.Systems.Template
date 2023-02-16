using Npgsql;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;

public class SampleDataDbInitializer : DbInitializerBase
{
    protected override IReadOnlyCollection<DbInitializerBase> Initializers { get; } =
        new List<DbInitializerBase> { new SchemaInitializer() };

    public override async Task InitializeAsync(NpgsqlConnection connection)
    {
        await base.InitializeAsync(connection);

        string schemaSql = EmbeddedFilesReader.GetContent("Common/Db/Assets/sample_data.sql");

        await using NpgsqlCommand command = new(schemaSql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
