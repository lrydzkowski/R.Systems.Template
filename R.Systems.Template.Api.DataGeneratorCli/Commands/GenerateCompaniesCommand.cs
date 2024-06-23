using CommandDotNet;
using R.Systems.Template.Api.DataGeneratorCli.Services;

namespace R.Systems.Template.Api.DataGeneratorCli.Commands;

[Command("companies", Description = "Generate companies in database.")]
internal class GenerateCompaniesCommand
{
    private readonly CompanyService _companyService;

    public GenerateCompaniesCommand(CompanyService companyService)
    {
        _companyService = companyService;
    }

    [DefaultCommand]
    public async Task ExecuteAsync(
        [Option("number-of-companies")] int numberOfCompanies = 1000,
        [Option("number-of-employees")] int numberOfEmployees = 10000
    )
    {
        await _companyService.CreateCompaniesAsync(numberOfCompanies, numberOfEmployees);
    }
}
