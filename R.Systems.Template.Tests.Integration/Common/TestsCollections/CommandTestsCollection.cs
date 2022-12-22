using R.Systems.Template.Tests.Integration.Common.Db;
using R.Systems.Template.Tests.Integration.Common.WebApplication;

namespace R.Systems.Template.Tests.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class CommandTestsCollection : ICollectionFixture<WebApiFactoryWithDb<SampleDataDbInitializer>>
{
    public const string CollectionName = "CommandTests";
}
