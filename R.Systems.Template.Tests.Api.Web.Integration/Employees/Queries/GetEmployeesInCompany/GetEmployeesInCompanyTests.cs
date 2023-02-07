using System.Linq.Dynamic.Core;
using System.Net;
using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Queries.GetEmployeesInCompany;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetEmployeesInCompanyTests
{
    public GetEmployeesInCompanyTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        RestClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetEmployeesInCompany_ShouldReturnEmployees_WhenEmployeesExist()
    {
        int companyId = IdGenerator.GetCompanyId(1);
        IQueryable<Employee> expectedEmployees = EmployeesSampleData.Employees
            .Where(x => x.CompanyId == companyId)
            .AsQueryable();
        ListInfo<Employee> expectedResponse = new()
        {
            Data = expectedEmployees.ToList(),
            Count = expectedEmployees.Count()
        };
        RestRequest restRequest = new($"/companies/{companyId}/employees");

        RestResponse<ListInfo<Employee>> response = await RestClient.ExecuteAsync<ListInfo<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse);
    }

    [Theory]
    [InlineData(2, 4)]
    [InlineData(1, 10)]
    public async Task GetEmployeesInCompany_ShouldReturnPaginatedEmployees_WhenPaginationParametersArePassed(
        int page,
        int pageSize
    )
    {
        int companyId = IdGenerator.GetCompanyId(2);
        IQueryable<Employee> expectedEmployees = EmployeesSampleData.Employees
            .Where(x => x.CompanyId == companyId)
            .OrderBy(x => x.EmployeeId)
            .AsQueryable();
        ListInfo<Employee> expectedResponse = new()
        {
            Data = expectedEmployees
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList(),
            Count = expectedEmployees.Count()
        };
        RestRequest restRequest = new($"/companies/{companyId}/employees");
        restRequest.AddQueryParameter(nameof(page), page);
        restRequest.AddQueryParameter(nameof(pageSize), pageSize);

        RestResponse<ListInfo<Employee>> response = await RestClient.ExecuteAsync<ListInfo<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
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
        int companyId = IdGenerator.GetCompanyId(2);
        IQueryable<Employee> expectedEmployees = EmployeesSampleData.Employees.AsQueryable()
            .Where(x => x.CompanyId == companyId)
            .OrderBy($"{sortingFieldName} {sortingOrder}")
            .ThenBy(x => x.EmployeeId)
            .AsQueryable();
        ListInfo<Employee> expectedResponse = new()
        {
            Data = expectedEmployees.ToList(),
            Count = expectedEmployees.Count()
        };
        RestRequest restRequest = new($"/companies/{companyId}/employees");
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);

        RestResponse<ListInfo<Employee>> response = await RestClient.ExecuteAsync<ListInfo<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData("John")]
    [InlineData("oh")]
    [InlineData("ohn")]
    [InlineData("dez")]
    public async Task GetEmployeesInCompany_ShouldReturnFilteredEmployees_WhenSearchParametersArePassed(
        string searchQuery
    )
    {
        int companyId = IdGenerator.GetCompanyId(2);
        IQueryable<Employee> expectedEmployees = EmployeesSampleData.Employees
            .Where(x => x.CompanyId == companyId)
            .Where(
                x => x.FirstName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
                     || x.LastName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
            )
            .AsQueryable();
        ListInfo<Employee> expectedResponse = new()
        {
            Data = expectedEmployees.ToList(),
            Count = expectedEmployees.Count()
        };
        RestRequest restRequest = new($"/companies/{companyId}/employees");
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<ListInfo<Employee>> response = await RestClient.ExecuteAsync<ListInfo<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task GetEmployees_ShouldReturnCorrectEmployees_WhenParametersArePassed()
    {
        int companyId = IdGenerator.GetCompanyId(2);
        int page = 1;
        int pageSize = 2;
        string sortingFieldName = "firstName";
        string sortingOrder = "asc";
        string searchQuery = "o";

        IQueryable<Employee> expectedEmployees = EmployeesSampleData.Employees
            .Where(x => x.CompanyId == companyId)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.EmployeeId)
            .Where(
                x => x.FirstName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
                     || x.LastName.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase)
            )
            .AsQueryable();
        ListInfo<Employee> expectedResponse = new()
        {
            Data = expectedEmployees
                // ReSharper disable once UselessBinaryOperation
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList(),
            Count = expectedEmployees.Count()
        };
        RestRequest restRequest = new($"/companies/{companyId}/employees");
        restRequest.AddQueryParameter(nameof(page), page);
        restRequest.AddQueryParameter(nameof(pageSize), pageSize);
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<ListInfo<Employee>> response = await RestClient.ExecuteAsync<ListInfo<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }
}
