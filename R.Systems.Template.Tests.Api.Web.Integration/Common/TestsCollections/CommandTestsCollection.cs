using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class CommandTestsCollection : ICollectionFixture<WebApiFactoryWithDb<SampleDataDbInitializer>>
{
    public const string CollectionName = "WebApiCommandTests";
}
