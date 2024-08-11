using R.Systems.Template.Infrastructure.CosmosDb.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.CosmosDb;

internal class CosmosDbOptionsData : CosmosDbOptions, IOptionsData
{
    public CosmosDbOptionsData()
    {
        AccountUri = "https://cosmos-db-account.com";
        DatabaseName = "database-name";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(AccountUri)}"] = AccountUri,
            [$"{Position}:{nameof(DatabaseName)}"] = DatabaseName
        };
    }
}
