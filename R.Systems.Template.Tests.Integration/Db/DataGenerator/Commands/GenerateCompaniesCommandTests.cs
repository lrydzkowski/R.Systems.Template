using CommandDotNet;
using CommandDotNet.TestTools;
using FluentAssertions;
using R.Systems.Template.Persistence.Db.DataGenerator;
using R.Systems.Template.Tests.Integration.Common.Builders;

namespace R.Systems.Template.Tests.Integration.Db.DataGenerator.Commands;

public class GenerateCompaniesCommandTests
{
    [Fact]
    public void GenerateData_ShouldGenerateData_WhenCorrectArgumentArePassed()
    {
        TestConsole testConsole = new();
        AppRunnerFactory appRunnerFactory = new AppRunnerFactory().WithDatabaseInMemory().WithTestConsole(testConsole);
        AppRunner appRunner = appRunnerFactory.Create();

        AppRunnerResult generateResult = appRunner.RunInMem(
            "generate companies --number-of-companies 10 --number-of-employees 20"
        );

        generateResult.ExitCode.Should().Be(0);

        AppRunnerResult getResult = appRunner.RunInMem("get companies");

        getResult.ExitCode.Should().Be(0);

        //IConsole? console = (IConsole?)appRunnerFactory.ServiceProvider?.GetService(typeof(IConsole));
    }
}
