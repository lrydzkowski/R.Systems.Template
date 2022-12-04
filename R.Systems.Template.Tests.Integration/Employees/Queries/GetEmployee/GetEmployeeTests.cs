using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Persistence.Db.Common.Entities;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Integration.Common.Factories;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Employees.Queries.GetEmployee;

public class GetEmployeeTests : IClassFixture<WebApiFactory>
{
    private readonly string _endpointUrlPath = "/employees";

    public GetEmployeeTests(WebApiFactory webApiFactory)
    {
        WebApiFactory = webApiFactory;
    }

    private WebApiFactory WebApiFactory { get; }

    [Fact]
    public async Task GetEmployee_ShouldReturnEmployee_WhenEmployeeExists()
    {
        EmployeeEntity expectedEmployeeEntity = EmployeesSampleData.Data[0];
        Employee expectedEmployee = new()
        {
            EmployeeId = (int)expectedEmployeeEntity.Id!,
            FirstName = expectedEmployeeEntity.FirstName,
            LastName = expectedEmployeeEntity.LastName,
            CompanyId = expectedEmployeeEntity.CompanyId
        };
        RestClient restClient = WebApiFactory.CreateRestClient();
        RestRequest restRequest = new($"{_endpointUrlPath}/{expectedEmployee.EmployeeId}");

        RestResponse<Employee> response = await restClient.ExecuteAsync<Employee>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployee);
    }

    [Fact]
    public async Task GetEmployee_ShouldReturn404_WhenEmployeeNotExist()
    {
        int employeeId = 100;
        RestClient restClient = WebApiFactory.CreateRestClient();
        RestRequest restRequest = new($"{_endpointUrlPath}/{employeeId}");

        RestResponse<ErrorInfo> response = await restClient.ExecuteAsync<ErrorInfo>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Data.Should()
            .BeEquivalentTo(
                new ErrorInfo
                {
                    PropertyName = "Employee",
                    ErrorMessage = "Employee doesn't exist.",
                    ErrorCode = "NotExist"
                },
                options => options.Including(x => x.PropertyName).Including(x => x.ErrorMessage)
            );
    }
}
