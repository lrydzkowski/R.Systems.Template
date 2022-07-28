using FluentAssertions;
using R.Systems.Template.Core.Common.DataTransferObjects;
using R.Systems.Template.FunctionalTests.Common.Factories;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Template.FunctionalTests.Companies.Queries.GetCompany;

public class GetCompanyTests : IClassFixture<WebApiFactory<Program>>
{
    private readonly string _endpointUrlPath = "/companies";

    public GetCompanyTests(WebApiFactory<Program> webApiFactory)
    {
        RestClient = new RestClient(webApiFactory.CreateClient());
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetCompany_ShouldReturnCompany_WhenCompanyExists()
    {
        int companyId = 1;
        CompanyDto expectedCompany = new()
        {
            CompanyId = 1,
            Name = "Meta",
            Employees = new List<EmployeeDto>()
            {
                new()
                {
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Doe"
                }
            }
        };
        RestRequest restRequest = new($"{_endpointUrlPath}/{companyId}");

        RestResponse<CompanyDto> response = await RestClient.ExecuteAsync<CompanyDto>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedCompany);
    }

    [Fact]
    public async Task GetCompany_ShouldReturn404_WhenCompanyNotExist()
    {
        int companyId = 5;
        RestRequest restRequest = new($"{_endpointUrlPath}/{companyId}");

        RestResponse<CompanyDto> response = await RestClient.ExecuteAsync<CompanyDto>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Data.Should().BeNull();
    }
}
