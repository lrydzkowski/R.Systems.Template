using Microsoft.Data.SqlClient;

namespace R.Systems.Template.Tests.Core.Integration.Common.Db;

public abstract class DbInitializerBase
{
    protected virtual IReadOnlyCollection<DbInitializerBase> Initializers { get; } = new List<DbInitializerBase>();

    public virtual async Task InitializeAsync(SqlConnection connection)
    {
        foreach (DbInitializerBase initializer in Initializers)
        {
            await initializer.InitializeAsync(connection);
        }
    }
}
