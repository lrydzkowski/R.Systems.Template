using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace R.Systems.Template.Tests.Core.Integration.Common.Db;

public class SampleDataDbInitializer : DbInitializerBase
{
    protected override IReadOnlyCollection<DbInitializerBase> Initializers { get; } =
        new List<DbInitializerBase> { new SchemaInitializer() };

    public override void Initialize(SqlConnection connection)
    {
        base.Initialize(connection);

        string sql = EmbeddedFilesReader.GetContent("Common/Db/Assets/sample_data.sql");

        ServerConnection serverConnection = new(connection);
        Server server = new(serverConnection);

        server.ConnectionContext.ExecuteNonQuery(sql);
    }
}
