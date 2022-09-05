using CommandDotNet;
using R.Systems.Template.Persistence.Db.DataGenerator.Services;

namespace R.Systems.Template.Persistence.Db.DataGenerator.Commands;

[Command(
    name: "companies",
    Description = "Generate companies in database."
)]
internal class GenerateCompaniesCommand
{
    public GenerateCompaniesCommand(CompanyService companyService)
    {
        CompanyService = companyService;
    }

    private CompanyService CompanyService { get; }

    [DefaultCommand]
    public async Task ExecuteAsync(
        [Option("number-of-companies")] int numberOfCompanies = 1000,
        [Option("number-of-employees")] int numberOfEmployees = 10000
    )
    {
        await CompanyService.CreateCompaniesAsync(numberOfCompanies, numberOfEmployees);
    }
}
