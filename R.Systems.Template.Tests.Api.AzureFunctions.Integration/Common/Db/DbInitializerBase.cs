using Npgsql;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;

public abstract class DbInitializerBase
{
    protected virtual IReadOnlyCollection<DbInitializerBase> Initializers { get; } = new List<DbInitializerBase>();

    public virtual async Task InitializeAsync(NpgsqlConnection connection)
    {
        foreach (DbInitializerBase initializer in Initializers)
        {
            await initializer.InitializeAsync(connection);
        }
    }
}
