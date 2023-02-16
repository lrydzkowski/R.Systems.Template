using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Function;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class QueryTestsCollection : ICollectionFixture<FunctionFactory<SampleDataDbInitializer>>
{
    public const string CollectionName = "AzureFunctionsQueryTests";
}
