using Npgsql;

namespace R.Systems.Template.Tests.Core.Integration.Common.Db;

public class NoDataDbInitializer : DbInitializerBase
{
    protected override IReadOnlyCollection<DbInitializerBase> Initializers { get; } =
        new List<DbInitializerBase> { new SchemaInitializer() };

    public override async Task InitializeAsync(NpgsqlConnection connection)
    {
        await base.InitializeAsync(connection);

        string schemaSql = EmbeddedFilesReader.GetContent("Common/Db/Assets/no_data.sql");

        await using NpgsqlCommand command = new(schemaSql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
