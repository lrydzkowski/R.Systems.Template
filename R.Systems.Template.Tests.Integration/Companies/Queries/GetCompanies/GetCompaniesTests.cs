using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Integration.Common.Factories;
using RestSharp;
using System.Linq.Dynamic.Core;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Companies.Queries.GetCompanies;

public class GetEmployeesTests : IClassFixture<WebApiFactory>
{
    private readonly string _endpointUrlPath = "/companies";

    public GetEmployeesTests(WebApiFactory webApiFactory)
    {
        WebApiFactory = webApiFactory;
    }

    private WebApiFactory WebApiFactory { get; }

    [Fact]
    public async Task GetCompanies_ShouldReturnCompanies_WhenCompaniesExist()
    {
        List<Company> expectedCompanies = CompaniesSampleData.Companies;
        RestClient restClient = WebApiFactory.CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Company>> response = await restClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedCompanies);
    }

    [Fact]
    public async Task GetCompanies_ShouldReturnEmptyList_WhenCompaniesNotExist()
    {
        RestClient restClient = WebApiFactory.WithoutData().CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Company>> response = await restClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(new List<Company>());
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(0, 10)]
    public async Task GetCompanies_ShouldReturnPaginatedCompanies_WhenPaginationParametersArePassed(
        int firstIndex,
        int numberOfRows
    )
    {
        List<Company> expectedCompanies = CompaniesSampleData.Companies.OrderBy(x => x.CompanyId)
            .Skip(firstIndex)
            .Take(numberOfRows)
            .ToList();
        RestClient restClient = WebApiFactory.CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(firstIndex), firstIndex);
        restRequest.AddQueryParameter(nameof(numberOfRows), numberOfRows);

        RestResponse<List<Company>> response = await restClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should()
            .BeEquivalentTo(
                expectedCompanies,
                config => config.WithStrictOrdering()
                    .Excluding(
                        x => x.Employees
                    )
            );
    }

    [Theory]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    public async Task GetCompanies_ShouldReturnSortedCompanies_WhenSortingParametersArePassed(
        string sortingFieldName,
        string sortingOrder
    )
    {
        List<Company> expectedCompanies = CompaniesSampleData.Companies.AsQueryable()
            .OrderBy($"{sortingFieldName} {sortingOrder}")
            .ToList();
        RestClient restClient = WebApiFactory.CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);

        RestResponse<List<Company>> response = await restClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should()
            .BeEquivalentTo(
                expectedCompanies,
                config => config.WithStrictOrdering()
                    .Excluding(
                        x => x.Employees
                    )
            );
    }

    [Theory]
    [InlineData("Microsoft")]
    [InlineData("microsofT")]
    [InlineData("croso")]
    [InlineData("o")]
    public async Task GetCompanies_ShouldReturnFilteredCompanies_WhenSearchParametersArePassed(string searchQuery)
    {
        List<Company> expectedCompanies = CompaniesSampleData.Companies
            .Where(x => x.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
        RestClient restClient = WebApiFactory.CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<List<Company>> response = await restClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should()
            .BeEquivalentTo(
                expectedCompanies,
                config => config.WithStrictOrdering()
                    .Excluding(
                        x => x.Employees
                    )
            );
    }

    [Fact]
    public async Task GetCompanies_ShouldReturnCorrectCompanies_WhenParametersArePassed()
    {
        int firstIndex = 1;
        int numberOfRows = 2;
        string sortingFieldName = "name";
        string sortingOrder = "asc";
        string searchQuery = "o";

        List<Company> expectedCompanies = CompaniesSampleData.Companies
            .OrderBy(x => x.Name)
            .Where(x => x.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
            .Skip(firstIndex)
            .Take(numberOfRows)
            .ToList();
        RestClient restClient = WebApiFactory.CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(firstIndex), firstIndex);
        restRequest.AddQueryParameter(nameof(numberOfRows), numberOfRows);
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<List<Company>> response = await restClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should()
            .BeEquivalentTo(
                expectedCompanies,
                config => config.WithStrictOrdering()
                    .Excluding(
                        x => x.Employees
                    )
            );
    }
}
