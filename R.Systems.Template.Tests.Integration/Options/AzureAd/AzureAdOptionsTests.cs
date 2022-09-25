using FluentValidation;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Integration.Options.AzureAd;

public class AzureAdOptionsTests
{
    public AzureAdOptionsTests(ITestOutputHelper output)
    {
        Output = output;
    }

    private ITestOutputHelper Output { get; }

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
            () => new WebApiFactory<Program>().WithCustomOptions(options).CreateRestClient()
        );

        Assert.Equal(expectedErrorMessage, ex.Message);
    }
}
