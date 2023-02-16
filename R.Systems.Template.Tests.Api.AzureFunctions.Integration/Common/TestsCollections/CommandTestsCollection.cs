using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Function;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class CommandTestsCollection : ICollectionFixture<FunctionFactory<SampleDataDbInitializer>>
{
    public const string CollectionName = "AzureFunctionsCommandTests";
}
