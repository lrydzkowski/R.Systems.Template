using CommandDotNet;
using R.Systems.Template.Api.DataGeneratorCli.Services;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Api.DataGeneratorCli.Commands;

[Command("companies", Description = "Generate companies in database.")]
internal class GenerateCompaniesCommand
{
    private readonly IVersionedServiceFactory<ICompanyService> _serviceFactory;

    public GenerateCompaniesCommand(IVersionedServiceFactory<ICompanyService> serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    [DefaultCommand]
    public async Task ExecuteAsync(
        [Option("companies-count")] int numberOfCompanies = 1000,
        [Option("employees-count")] int numberOfEmployees = 10000,
        [Option("version")] string version = Versions.V1
    )
    {
        await _serviceFactory.GetService(version).CreateCompaniesAsync(numberOfCompanies, numberOfEmployees);
    }
}
