using System.Net;
using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;
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

    private readonly RestClient _restClient;

    public GetEmployeeTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        _restClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

    [Fact]
    public async Task GetEmployee_ShouldReturnEmployee_WhenEmployeeExists()
    {
        EmployeeEntity expectedEmployeeEntity = EmployeesSampleData.Data[0];
        Employee expectedEmployee = new()
        {
            EmployeeId = (Guid)expectedEmployeeEntity.Id!,
            FirstName = expectedEmployeeEntity.FirstName,
            LastName = expectedEmployeeEntity.LastName,
            CompanyId = expectedEmployeeEntity.CompanyId
        };
        RestRequest restRequest = new($"{_endpointUrlPath}/{expectedEmployee.EmployeeId}");
        RestResponse<Employee> response = await _restClient.ExecuteAsync<Employee>(restRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployee);
    }

    [Fact]
    public async Task GetEmployee_ShouldReturn404_WhenEmployeeNotExist()
    {
        string employeeId = "eff998c3-0d92-4c70-948f-607ada5e1153";
        RestRequest restRequest = new($"{_endpointUrlPath}/{employeeId}");
        RestResponse<ErrorInfo> response = await _restClient.ExecuteAsync<ErrorInfo>(restRequest);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Data.Should()
            .BeEquivalentTo(
                new ErrorInfo
                    { PropertyName = "Employee", ErrorMessage = "Employee doesn't exist.", ErrorCode = "NotExist" },
                options => options.Including(x => x.PropertyName).Including(x => x.ErrorMessage)
            );
    }
}
