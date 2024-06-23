using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class QueryWithoutDataTestsCollection : ICollectionFixture<WebApiFactoryWithDb<NoDataDbInitializer>>
{
    public const string CollectionName = "WebApiQueryWithoutDataTests";
}
