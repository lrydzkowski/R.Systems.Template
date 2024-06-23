using R.Systems.Template.Tests.Core.Integration.Common.Db;

namespace R.Systems.Template.Tests.Core.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class QueryTestsCollection : ICollectionFixture<SystemUnderTest<SampleDataDbInitializer>>
{
    public const string CollectionName = "CoreQueryTests";
}
