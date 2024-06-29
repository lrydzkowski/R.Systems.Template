using System.Net;
using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Infrastructure.Db.Common.Entities;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Queries.GetEmployeeInCompany;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetEmployeeInCompanyTests
{
    private readonly RestClient _restClient;

    public GetEmployeeInCompanyTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        _restClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

    [Fact]
    public async Task GetEmployeeInCompany_ShouldReturnEmployee_WhenEmployeeExists()
    {
        EmployeeEntity expectedEmployeeEntity = EmployeesSampleData.Data[0];
        Employee expectedEmployee = new()
        {
            EmployeeId = (long)expectedEmployeeEntity.Id!,
            FirstName = expectedEmployeeEntity.FirstName,
            LastName = expectedEmployeeEntity.LastName,
            CompanyId = expectedEmployeeEntity.CompanyId
        };
        RestRequest restRequest =
            new($"/companies/{expectedEmployee.CompanyId}/employees/{expectedEmployee.EmployeeId}");
        RestResponse<Employee> response = await _restClient.ExecuteAsync<Employee>(restRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployee);
    }

    [Fact]
    public async Task GetEmployeeInCompany_ShouldReturn404_WhenEmployeeNotExist()
    {
        EmployeeEntity employeeEntity = EmployeesSampleData.Data[0];
        RestRequest restRequest = new($"/companies/{employeeEntity.CompanyId + 1}/employees/{employeeEntity.Id}");
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
