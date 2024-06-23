using FluentValidation;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.ConnectionStrings;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class ConnectionStringsOptionsTests
{
    private readonly ITestOutputHelper _output;
    private readonly WebApiFactory _webApiFactory;

    public ConnectionStringsOptionsTests(
        ITestOutputHelper output,
        WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory
    )
    {
        _output = output;
        _webApiFactory = webApiFactory;
    }

    [Theory]
    [MemberData(
        nameof(ConnectionStringsOptionsIncorrectDataBuilder.Build),
        MemberType = typeof(ConnectionStringsOptionsIncorrectDataBuilder)
    )]
    public void InitApp_IncorrectAppSettings_ThrowsException(
        int id,
        Dictionary<string, string?> options,
        string expectedErrorMessage
    )
    {
        _output.WriteLine("Parameters set with id = {0}", id);
        ValidationException ex =
            Assert.Throws<ValidationException>(() => _webApiFactory.WithCustomOptions(options).CreateRestClient());
        Assert.Equal(expectedErrorMessage, ex.Message);
    }
}
