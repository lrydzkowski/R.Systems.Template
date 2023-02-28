using System.Net;
using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Infrastructure.Db.Postgres.Common.Entities;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Queries.GetEmployee;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetEmployeeTests
{
    private readonly string _endpointUrlPath = "/employees";

    public GetEmployeeTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        RestClient = webApiFactory.WithoutAuthentication().CreateRestClient();
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
