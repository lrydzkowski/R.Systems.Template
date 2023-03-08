using Microsoft.Data.SqlClient;

namespace R.Systems.Template.Tests.Core.Integration.Common.Db;

public class SchemaInitializer : DbInitializerBase
{
    public override async Task InitializeAsync(SqlConnection connection)
    {
        await base.InitializeAsync(connection);

        string schemaSql = EmbeddedFilesReader.GetContent("Common/Db/Assets/schema.sql");

        await using SqlCommand command = new(schemaSql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
