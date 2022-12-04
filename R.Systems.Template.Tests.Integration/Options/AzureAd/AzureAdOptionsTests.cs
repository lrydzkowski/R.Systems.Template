using FluentValidation;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Factories;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Integration.Options.AzureAd;

public class AzureAdOptionsTests : IClassFixture<WebApiFactory>
{
    public AzureAdOptionsTests(ITestOutputHelper output, WebApiFactory webApiFactory)
    {
        Output = output;
        WebApiFactory = webApiFactory;
    }

    private ITestOutputHelper Output { get; }
    private WebApiFactory WebApiFactory { get; }

    [Theory]
    [MemberData(
        nameof(AzureAdOptionsIncorrectDataBuilder.Build),
        MemberType = typeof(AzureAdOptionsIncorrectDataBuilder)
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
