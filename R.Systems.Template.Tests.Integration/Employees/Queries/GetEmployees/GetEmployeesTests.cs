using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common;
using R.Systems.Template.Tests.Integration.Common.Db;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Integration.Common.WebApplication;
using RestSharp;
using System.Linq.Dynamic.Core;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Employees.Queries.GetEmployees;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetEmployeesTests
{
    private readonly string _endpointUrlPath = "/employees";

    public GetEmployeesTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        RestClient = webApiFactory.CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetEmployees_ShouldReturnEmployees_WhenEmployeesExist()
    {
        List<Employee> expectedEmployees = EmployeesSampleData.Data
            .Where(x => x.Id != null && x.CompanyId != null)
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
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Employee>> response = await RestClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees);
    }

    [Theory]
    [InlineData(2, 4)]
    [InlineData(1, 10)]
    public async Task GetEmployees_ShouldReturnPaginatedEmployees_WhenPaginationParametersArePassed(
        int page,
        int pageSize
    )
    {
        List<Employee> expectedEmployees = EmployeesSampleData.Employees.OrderBy(x => x.EmployeeId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(page), page);
        restRequest.AddQueryParameter(nameof(pageSize), pageSize);

        RestResponse<List<Employee>> response = await RestClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees, config => config.WithStrictOrdering());
    }

    [Theory]
    [InlineData("firstName", "asc")]
    [InlineData("firstName", "desc")]
    [InlineData("lastName", "asc")]
    [InlineData("lastName", "desc")]
    public async Task GetEmployees_ShouldReturnSortedEmployees_WhenSortingParametersArePassed(
        string sortingFieldName,
        string sortingOrder
    )
    {
        List<Employee> expectedEmployees = EmployeesSampleData.Employees.AsQueryable()
            .OrderBy($"{sortingFieldName} {sortingOrder}")
            .ToList();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);

        RestResponse<List<Employee>> response = await RestClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees, config => config.WithStrictOrdering());
    }

    [Theory]
    [InlineData("John")]
    [InlineData("oh")]
    [InlineData("ohn")]
    [InlineData("dez")]
    public async Task GetEmployees_ShouldReturnFilteredEmployees_WhenSearchParametersArePassed(string searchQuery)
    {
        List<Employee> expectedEmployees = EmployeesSampleData.Employees.Where(
                x => x.FirstName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
                     || x.LastName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
            )
            .ToList();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<List<Employee>> response = await RestClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees, config => config.WithStrictOrdering());
    }

    [Fact]
    public async Task GetEmployees_ShouldReturnCorrectEmployees_WhenParametersArePassed()
    {
        int page = 1;
        int pageSize = 2;
        string sortingFieldName = "firstName";
        string sortingOrder = "asc";
        string searchQuery = "o";

        List<Employee> expectedEmployees = EmployeesSampleData.Employees
            .OrderBy(x => x.FirstName)
            .Where(
                x => x.FirstName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
                     || x.LastName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
            )
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(page), page);
        restRequest.AddQueryParameter(nameof(pageSize), pageSize);
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<List<Employee>> response = await RestClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees, config => config.WithStrictOrdering());
    }
}
