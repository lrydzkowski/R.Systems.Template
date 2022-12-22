using R.Systems.Template.Tests.Integration.Common.Db;
using R.Systems.Template.Tests.Integration.Common.WebApplication;

namespace R.Systems.Template.Tests.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class QueryWithoutDataTestsCollection
    : ICollectionFixture<WebApiFactoryWithDb<NoDataDbInitializer>>
{
    public const string CollectionName = "QueryWithoutDataTests";
}
