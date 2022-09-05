using Bogus;
using Microsoft.EntityFrameworkCore.Storage;
using R.Systems.Template.Persistence.Db.Common.Configurations;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.DataGenerator.Services;

internal class CompanyService
{
    public CompanyService(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task CreateCompaniesAsync(int numberOfCompanies, int numberOfEmployees)
    {
        await using IDbContextTransaction transaction = await DbContext.Database.BeginTransactionAsync();

        List<int> companiesIds = await CreateCompaniesAsync(numberOfCompanies);
        await CreateEmployeesAsync(numberOfEmployees, companiesIds);
        await transaction.CommitAsync();
    }

    private async Task<List<int>> CreateCompaniesAsync(int numberOfCompanies)
    {
        List<CompanyEntity> companies = BuildCompanyEntities(numberOfCompanies);
        await DbContext.Companies.AddRangeAsync(companies);
        await DbContext.SaveChangesAsync();

        return companies.Select(company => (int)company.Id!).ToList();
    }

    private List<CompanyEntity> BuildCompanyEntities(int numberOfCompanies)
    {
        List<string> companiesNames = new();

        return Enumerable.Range(1, numberOfCompanies)
            .Select(
                _ =>
                {
                    CompanyEntity companyEntity;
                    do
                    {
                        companyEntity = BuildCompanyEntityFaker().Generate();
                    } while (companiesNames.Contains(companyEntity.Name));

                    companiesNames.Add(companyEntity.Name);

                    return companyEntity;
                }
            )
            .ToList();
    }

    private Faker<CompanyEntity> BuildCompanyEntityFaker()
    {
        return new Faker<CompanyEntity>()
            .RuleFor(companyEntity => companyEntity.Name, faker => faker.Company.CompanyName());
    }

    private async Task CreateEmployeesAsync(int numberOfEmployees, List<int> companiesIds)
    {
        List<EmployeeEntity> employees = BuildEmployeeEntities(numberOfEmployees, companiesIds);
        await DbContext.Employees.AddRangeAsync(employees);
        await DbContext.SaveChangesAsync();
    }

    private List<EmployeeEntity> BuildEmployeeEntities(int numberOfEmployees, List<int> companiesIds)
    {
        int employeeId = EmployeeConfiguration.FirstAvailableId;

        return Enumerable.Range(employeeId, numberOfEmployees + employeeId)
            .Select(_ => BuildEmployeeEntityFaker(companiesIds).Generate())
            .ToList();
    }

    private Faker<EmployeeEntity> BuildEmployeeEntityFaker(List<int> companiesIds)
    {
        return new Faker<EmployeeEntity>()
            .RuleFor(employeeEntity => employeeEntity.FirstName, faker => faker.Name.FirstName())
            .RuleFor(employeeEntity => employeeEntity.LastName, faker => faker.Name.LastName())
            .RuleFor(
                employeeEntity => employeeEntity.CompanyId,
                faker => companiesIds[faker.Random.Number(0, companiesIds.Count - 1)]
            );
    }
}
