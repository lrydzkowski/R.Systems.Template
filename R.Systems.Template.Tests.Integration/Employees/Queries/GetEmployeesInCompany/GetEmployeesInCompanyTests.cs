using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Linq.Dynamic.Core;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Employees.Queries.GetEmployeesInCompany;

public class GetEmployeesInCompanyTests
{
    [Fact]
    public async Task GetEmployeesInCompany_ShouldReturnEmployees_WhenEmployeesExist()
    {
        int companyId = 1;
        List<Employee> expectedEmployees = EmployeesSampleData.Data
            .Where(x => x.Id != null && x.CompanyId == companyId)
            .Select(
                x => new Employee
                {
                    EmployeeId = (int)x.Id!,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CompanyId = (int)x.CompanyId!
                }
            )
            .ToList();
        RestClient restClient = new WebApiFactory<Program>().CreateRestClient();
        RestRequest restRequest = new($"/companies/{companyId}/employees");

        RestResponse<List<Employee>> response = await restClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees);
    }

    [Fact]
    public async Task GetEmployeesInCompany_ShouldReturnEmptyList_WhenEmployeesNotExist()
    {
        int companyId = 1;
        RestClient restClient = new WebApiFactory<Program>().WithoutData().CreateRestClient();
        RestRequest restRequest = new($"/companies/{companyId}/employees");

        RestResponse<List<Employee>> response = await restClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(new List<Employee>());
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(0, 10)]
    public async Task GetEmployeesInCompany_ShouldReturnPaginatedEmployees_WhenPaginationParametersArePassed(
        int firstIndex,
        int numberOfRows
    )
    {
        int companyId = 2;
        List<Employee> expectedEmployees = EmployeesSampleData.Employees
            .Where(x => x.CompanyId == companyId)
            .OrderBy(x => x.EmployeeId)
            .Skip(firstIndex)
            .Take(numberOfRows)
            .ToList();
        RestClient restClient = new WebApiFactory<Program>().CreateRestClient();
        RestRequest restRequest = new($"/companies/{companyId}/employees");
        restRequest.AddQueryParameter(nameof(firstIndex), firstIndex);
        restRequest.AddQueryParameter(nameof(numberOfRows), numberOfRows);

        RestResponse<List<Employee>> response = await restClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees, config => config.WithStrictOrdering());
    }

    [Theory]
    [InlineData("firstName", "asc")]
    [InlineData("firstName", "desc")]
    [InlineData("lastName", "asc")]
    [InlineData("lastName", "desc")]
    public async Task GetEmployeesInCompany_ShouldReturnSortedEmployees_WhenSortingParametersArePassed(
        string sortingFieldName,
        string sortingOrder
    )
    {
        int companyId = 2;
        List<Employee> expectedEmployees = EmployeesSampleData.Employees.AsQueryable()
            .Where(x => x.CompanyId == companyId)
            .OrderBy($"{sortingFieldName} {sortingOrder}")
            .ToList();
        RestClient restClient = new WebApiFactory<Program>().CreateRestClient();
        RestRequest restRequest = new($"/companies/{companyId}/employees");
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);

        RestResponse<List<Employee>> response = await restClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees, config => config.WithStrictOrdering());
    }

    [Theory]
    [InlineData("John")]
    [InlineData("oh")]
    [InlineData("ohn")]
    [InlineData("dez")]
    public async Task GetEmployeesInCompany_ShouldReturnFilteredEmployees_WhenSearchParametersArePassed(string searchQuery)
    {
        int companyId = 2;
        List<Employee> expectedEmployees = EmployeesSampleData.Employees
            .Where(x => x.CompanyId == companyId)
            .Where(
                x => x.FirstName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
                     || x.LastName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
            )
            .ToList();
        RestClient restClient = new WebApiFactory<Program>().CreateRestClient();
        RestRequest restRequest = new($"/companies/{companyId}/employees");
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<List<Employee>> response = await restClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees, config => config.WithStrictOrdering());
    }

    [Fact]
    public async Task GetEmployees_ShouldReturnCorrectEmployees_WhenParametersArePassed()
    {
        int companyId = 2;
        int firstIndex = 1;
        int numberOfRows = 2;
        string sortingFieldName = "firstName";
        string sortingOrder = "asc";
        string searchQuery = "o";

        List<Employee> expectedEmployees = EmployeesSampleData.Employees
            .Where(x => x.CompanyId == companyId)
            .OrderBy(x => x.FirstName)
            .Where(
                x => x.FirstName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
                     || x.LastName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
            )
            .Skip(firstIndex)
            .Take(numberOfRows)
            .ToList();
        RestClient restClient = new WebApiFactory<Program>().CreateRestClient();
        RestRequest restRequest = new($"/companies/{companyId}/employees");
        restRequest.AddQueryParameter(nameof(firstIndex), firstIndex);
        restRequest.AddQueryParameter(nameof(numberOfRows), numberOfRows);
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<List<Employee>> response = await restClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees, config => config.WithStrictOrdering());
    }
}
