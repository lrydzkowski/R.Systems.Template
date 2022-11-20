using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Employees.Queries.GetEmployees;

[Collection(QueryWithoutDataTestsCollection.CollectionName)]
public class GetEmployeesWithoutDataTests
{
    private readonly string _endpointUrlPath = "/employees";

    public GetEmployeesWithoutDataTests(WebApiFactory webApiFactory)
    {
        RestClient = webApiFactory.WithoutData().CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetEmployees_ShouldReturnEmptyList_WhenEmployeesNotExist()
    {
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Employee>> response = await RestClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(new List<Employee>());
    }
}
