using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Linq.Dynamic.Core;
using System.Net;

namespace R.Systems.Template.Tests.Api.Web.Integration.Companies.Queries.GetCompanies;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetCompaniesTests
{
    private readonly string _endpointUrlPath = "/companies";

    public GetCompaniesTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        RestClient = webApiFactory.CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetCompanies_ShouldReturnCompanies_WhenCompaniesExist()
    {
        List<Company> expectedCompanies = CompaniesSampleData.Companies;
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Company>> response = await RestClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedCompanies);
    }

    [Theory]
    [InlineData(2, 4)]
    [InlineData(1, 10)]
    public async Task GetCompanies_ShouldReturnPaginatedCompanies_WhenPaginationParametersArePassed(
        int page,
        int pageSize
    )
    {
        List<Company> expectedCompanies = CompaniesSampleData.Companies.OrderBy(x => x.CompanyId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(page), page);
        restRequest.AddQueryParameter(nameof(pageSize), pageSize);

        RestResponse<List<Company>> response = await RestClient.ExecuteAsync<List<Company>>(restRequest);

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
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);

        RestResponse<List<Company>> response = await RestClient.ExecuteAsync<List<Company>>(restRequest);

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
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<List<Company>> response = await RestClient.ExecuteAsync<List<Company>>(restRequest);

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
        int page = 2;
        int pageSize = 2;
        string sortingFieldName = "name";
        string sortingOrder = "asc";
        string searchQuery = "o";

        List<Company> expectedCompanies = CompaniesSampleData.Companies
            .OrderBy(x => x.Name)
            .Where(x => x.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
            // ReSharper disable once UselessBinaryOperation
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(page), page);
        restRequest.AddQueryParameter(nameof(pageSize), pageSize);
        restRequest.AddQueryParameter(nameof(sortingFieldName), sortingFieldName);
        restRequest.AddQueryParameter(nameof(sortingOrder), sortingOrder);
        restRequest.AddQueryParameter(nameof(searchQuery), searchQuery);

        RestResponse<List<Company>> response = await RestClient.ExecuteAsync<List<Company>>(restRequest);

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
