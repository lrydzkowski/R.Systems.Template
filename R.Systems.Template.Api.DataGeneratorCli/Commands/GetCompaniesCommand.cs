using CommandDotNet;
using R.Systems.Template.Api.DataGeneratorCli.Services;
using R.Systems.Template.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Template.Api.DataGeneratorCli.Commands;

[Command(
    name: "companies",
    Description = "Get companies from database."
)]
internal class GetCompaniesCommand
{
    public GetCompaniesCommand(CompanyService companyService, IConsole console)
    {
        CompanyService = companyService;
        Console = console;
    }

    private CompanyService CompanyService { get; }
    private IConsole Console { get; }

    [DefaultCommand]
    public async Task ExecuteAsync()
    {
        List<CompanyEntity> companies = await CompanyService.GetCompaniesAsync();
        foreach (CompanyEntity company in companies)
        {
            Console.WriteLine($"{company.Id} | {company.Name}");
            foreach (EmployeeEntity employee in company.Employees)
            {
                Console.WriteLine($"    {employee.Id} | {employee.FirstName} | {employee.LastName}");
            }
        }
    }
}
