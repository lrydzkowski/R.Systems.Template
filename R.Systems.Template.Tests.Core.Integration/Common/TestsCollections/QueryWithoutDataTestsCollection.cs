using R.Systems.Template.Tests.Core.Integration.Common.Db;

namespace R.Systems.Template.Tests.Core.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class QueryWithoutDataTestsCollection : ICollectionFixture<SystemUnderTest<NoDataDbInitializer>>
{
    public const string CollectionName = "CoreQueryWithoutDataTests";
}
