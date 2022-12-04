using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Factories;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Companies.Queries.GetCompany;

public class GetEmployeeTests : IClassFixture<WebApiFactory>
{
    private readonly string _endpointUrlPath = "/companies";

    public GetEmployeeTests(WebApiFactory webApiFactory)
    {
        WebApiFactory = webApiFactory;
    }

    private WebApiFactory WebApiFactory { get; }

    [Fact]
    public async Task GetCompany_ShouldReturnCompany_WhenCompanyExists()
    {
        int companyId = 1;
        Company expectedCompany = new()
        {
            CompanyId = 1,
            Name = "Meta",
            Employees = new List<Employee>()
            {
                new()
                {
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Doe"
                }
            }
        };
        RestClient restClient = WebApiFactory.CreateRestClient();
        RestRequest restRequest = new($"{_endpointUrlPath}/{companyId}");

        RestResponse<Company> response = await restClient.ExecuteAsync<Company>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedCompany);
    }

    [Fact]
    public async Task GetCompany_ShouldReturn404_WhenCompanyNotExist()
    {
        int companyId = 10;
        RestClient restClient = WebApiFactory.CreateRestClient();
        RestRequest restRequest = new($"{_endpointUrlPath}/{companyId}");

        RestResponse<ErrorInfo> response = await restClient.ExecuteAsync<ErrorInfo>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Data.Should()
            .BeEquivalentTo(
                new ErrorInfo
                {
                    PropertyName = "Company",
                    ErrorMessage = "Company doesn't exist.",
                    ErrorCode = "NotExist"
                },
                options => options.Including(x => x.PropertyName).Including(x => x.ErrorMessage)
            );
    }
}
