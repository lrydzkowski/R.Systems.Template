using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Employees.Queries.GetEmployees;

public class GetEmployeesTests
{
    private readonly string _endpointUrlPath = "/employees";

    [Fact]
    public async Task GetEmployees_ShouldReturnEmployees_WhenEmployeesExist()
    {
        List<Employee> expectedEmployees = EmployeesSampleData.Data
            .Where(x => x.Id != null && x.CompanyId != null)
            .Select(
                x => new Employee
                {
                    EmployeeId = (int)x.Id!,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CompanyId = (int)x.CompanyId!
                }
            )
            .ToList();
        RestClient restClient = new WebApiFactory<Program>().CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Employee>> response = await restClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedEmployees);
    }

    [Fact]
    public async Task GetEmployees_ShouldReturnEmptyList_WhenEmployeesNotExist()
    {
        RestClient restClient = new WebApiFactory<Program>().WithoutData().CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Employee>> response = await restClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(new List<Employee>());
    }
}
