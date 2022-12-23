using CommandDotNet;
using CommandDotNet.TestTools;
using FluentAssertions;
using R.Systems.Template.Tests.Api.DataGeneratorCli.Integration.Common;

namespace R.Systems.Template.Tests.Api.DataGeneratorCli.Integration.DataGenerator.Commands;

[Trait(TestConstants.Category, "DataGenerator")]
public class GenerateCompaniesCommandTests : IClassFixture<ConsoleAppRunnerFactory>
{
    public GenerateCompaniesCommandTests(ConsoleAppRunnerFactory consoleAppRunnerFactory)
    {
        ConsoleAppRunnerFactory = consoleAppRunnerFactory;
    }

    private ConsoleAppRunnerFactory ConsoleAppRunnerFactory { get; }

    [Fact]
    public async Task GenerateData_ShouldGenerateData_WhenCorrectArgumentArePassed()
    {
        AppRunner appRunner = await ConsoleAppRunnerFactory.WithTestConsole(new TestConsole()).CreateAsync();

        AppRunnerResult generateResult = appRunner.RunInMem(
            "generate companies --number-of-companies 10 --number-of-employees 20"
        );

        generateResult.ExitCode.Should().Be(0);

        AppRunnerResult getResult = appRunner.RunInMem("get companies");

        getResult.ExitCode.Should().Be(0);
    }
}
