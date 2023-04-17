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
        int numOfCompanies = 10;
        int numOfEmployees = 20;
        TestConsole testConsole = new();
        AppRunner appRunner = await ConsoleAppRunnerFactory.WithTestConsole(testConsole).CreateAsync();

        AppRunnerResult generateResult = appRunner.RunInMem(
            $"generate companies --number-of-companies {numOfCompanies} --number-of-employees {numOfEmployees}"
        );

        generateResult.ExitCode.Should().Be(0);

        AppRunnerResult getResult = appRunner.RunInMem("get companies");

        getResult.ExitCode.Should().Be(0);

        string? console = testConsole.Out.ToString();
        List<string> consoleLines = console?.Split("\n").Select(x => x.Trim()).ToList() ?? new List<string>();

        consoleLines.Should().HaveCount(numOfCompanies + numOfEmployees);
    }
}
