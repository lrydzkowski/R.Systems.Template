using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Persistence.Db.Common.Entities;
using R.Systems.Template.Tests.Integration.Common;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Employees.Queries.GetEmployee;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetEmployeeTests
{
    private readonly string _endpointUrlPath = "/employees";

    public GetEmployeeTests(WebApiFactory webApiFactory)
    {
        RestClient = webApiFactory.CreateRestClient();
    }

    private RestClient RestClient { get; }

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
        RestRequest restRequest = new($"{_endpointUrlPath}/{expectedEmployee.EmployeeId}");

        RestResponse<Employee> response = await RestClient.ExecuteAsync<Employee>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployee);
    }

    [Fact]
    public async Task GetEmployee_ShouldReturn404_WhenEmployeeNotExist()
    {
        int employeeId = 100;
        RestRequest restRequest = new($"{_endpointUrlPath}/{employeeId}");

        RestResponse<ErrorInfo> response = await RestClient.ExecuteAsync<ErrorInfo>(restRequest);

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
