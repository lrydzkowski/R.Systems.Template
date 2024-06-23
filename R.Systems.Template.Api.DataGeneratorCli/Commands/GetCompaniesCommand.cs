using CommandDotNet;
using R.Systems.Template.Api.DataGeneratorCli.Services;
using R.Systems.Template.Infrastructure.Db.Common.Entities;

namespace R.Systems.Template.Api.DataGeneratorCli.Commands;

[Command("companies", Description = "Get companies from database.")]
internal class GetCompaniesCommand
{
    private readonly CompanyService _companyService;
    private readonly IConsole _console;

    public GetCompaniesCommand(CompanyService companyService, IConsole console)
    {
        _companyService = companyService;
        _console = console;
    }

    [DefaultCommand]
    public async Task ExecuteAsync()
    {
        List<CompanyEntity> companies = await _companyService.GetCompaniesAsync();
        foreach (CompanyEntity company in companies)
        {
            _console.WriteLine($"{company.Id} | {company.Name}");
            foreach (EmployeeEntity employee in company.Employees)
            {
                _console.WriteLine($"    {employee.Id} | {employee.FirstName} | {employee.LastName}");
            }
        }
    }
}
