using Npgsql;

namespace R.Systems.Template.Tests.Core.Integration.Common.Db;

public class SampleDataDbInitializer : DbInitializerBase
{
    protected override IReadOnlyCollection<DbInitializerBase> Initializers { get; } = new List<DbInitializerBase>
    {
        new SchemaInitializer()
    };

    public override async Task InitializeAsync(NpgsqlConnection connection)
    {
        await base.InitializeAsync(connection);
        string sql = EmbeddedFilesReader.GetContent("Common/Db/Assets/sample_data.sql");
        await using NpgsqlCommand command = new(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
