using Npgsql;

namespace R.Systems.Template.Tests.Core.Integration.Common.Db;

public class SchemaInitializer : DbInitializerBase
{
    public override async Task InitializeAsync(NpgsqlConnection connection)
    {
        await base.InitializeAsync(connection);

        string sql = EmbeddedFilesReader.GetContent("Common/Db/Assets/schema.sql");

        await using NpgsqlCommand command = new(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
