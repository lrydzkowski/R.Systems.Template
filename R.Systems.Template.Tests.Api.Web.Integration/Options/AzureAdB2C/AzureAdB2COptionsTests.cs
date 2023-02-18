using FluentValidation;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAd;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAdB2C;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class AzureAdB2COptionsTests
{
    public AzureAdB2COptionsTests(ITestOutputHelper output, WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        Output = output;
        WebApiFactory = webApiFactory;
    }

    private ITestOutputHelper Output { get; }
    private WebApiFactory WebApiFactory { get; }

    [Theory]
    [MemberData(
        nameof(AzureAdB2COptionsIncorrectDataBuilder.Build),
        MemberType = typeof(AzureAdB2COptionsIncorrectDataBuilder)
    )]
    public void InitApp_IncorrectAppSettings_ThrowsException(
        int id,
        Dictionary<string, string?> options,
        string expectedErrorMessage
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        ValidationException ex = Assert.Throws<ValidationException>(
            () => WebApiFactory.WithCustomOptions(options).CreateRestClient()
        );

        Assert.Equal(expectedErrorMessage, ex.Message);
    }
}
