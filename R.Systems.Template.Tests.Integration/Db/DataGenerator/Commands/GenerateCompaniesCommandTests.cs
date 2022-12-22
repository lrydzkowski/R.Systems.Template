using CommandDotNet;
using CommandDotNet.TestTools;
using FluentAssertions;
using R.Systems.Template.Tests.Integration.Common;
using R.Systems.Template.Tests.Integration.Common.ConsoleAppRunner;

namespace R.Systems.Template.Tests.Integration.Db.DataGenerator.Commands;

[Trait(TestConstants.Category, "DataGenerator")]
public class GenerateCompaniesCommandTests : IClassFixture<ConsoleAppRunnerFactory>
{
    public GenerateCompaniesCommandTests(ConsoleAppRunnerFactory consoleAppRunnerFactory)
    {
        ConsoleAppRunnerFactory = consoleAppRunnerFactory;
    }

    private ConsoleAppRunnerFactory ConsoleAppRunnerFactory { get; }

    [Fact]
    public void GenerateData_ShouldGenerateData_WhenCorrectArgumentArePassed()
    {
        AppRunner appRunner = ConsoleAppRunnerFactory.WithTestConsole(new TestConsole()).Create();

        AppRunnerResult generateResult = appRunner.RunInMem(
            "generate companies --number-of-companies 10 --number-of-employees 20"
        );

        generateResult.ExitCode.Should().Be(0);

        AppRunnerResult getResult = appRunner.RunInMem("get companies");

        getResult.ExitCode.Should().Be(0);

        //IConsole? console = (IConsole?)appRunnerFactory.ServiceProvider?.GetService(typeof(IConsole));
    }
}
