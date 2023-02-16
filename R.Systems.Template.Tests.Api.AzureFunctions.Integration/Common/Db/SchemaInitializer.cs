using Npgsql;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;

public class SchemaInitializer : DbInitializerBase
{
    public override async Task InitializeAsync(NpgsqlConnection connection)
    {
        await base.InitializeAsync(connection);

        string schemaSql = EmbeddedFilesReader.GetContent("Common/Db/Assets/schema.sql");

        await using NpgsqlCommand command = new(schemaSql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
