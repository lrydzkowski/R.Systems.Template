namespace R.Systems.Template.Api.DataGeneratorCli.Services;

internal interface ICompanyService : IVersionedService
{
    Task CreateCompaniesAsync(int numberOfCompanies, int numberOfEmployees);
}
