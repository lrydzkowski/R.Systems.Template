using R.Systems.Template.Tests.Core.Integration.Common.Db;

namespace R.Systems.Template.Tests.Core.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class CommandTestsCollection : ICollectionFixture<SystemUnderTest<SampleDataDbInitializer>>
{
    public const string CollectionName = "CoreCommandTests";
}
