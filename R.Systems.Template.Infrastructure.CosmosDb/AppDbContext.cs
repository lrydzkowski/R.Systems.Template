using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Options;

namespace R.Systems.Template.Infrastructure.CosmosDb;

internal class AppDbContext
{
    public AppDbContext(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options)
    {
        Database = cosmosClient.GetDatabase(options.Value.DatabaseName);
    }

    public Database Database { get; }
}
