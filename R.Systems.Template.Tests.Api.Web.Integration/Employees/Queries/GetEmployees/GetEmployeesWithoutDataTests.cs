using System.Net;
using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Queries.GetEmployees;

[Collection(QueryWithoutDataTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryWithoutDataTestsCollection.CollectionName)]
public class GetEmployeesWithoutDataTests
{
    private readonly string _endpointUrlPath = "/employees";

    private readonly RestClient _restClient;

    public GetEmployeesWithoutDataTests(WebApiFactoryWithDb<NoDataDbInitializer> webApiFactory)
    {
        _restClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

    [Fact]
    public async Task GetEmployees_ShouldReturnEmptyList_WhenEmployeesNotExist()
    {
        RestRequest restRequest = new(_endpointUrlPath);
        RestResponse<ListInfo<Employee>> response = await _restClient.ExecuteAsync<ListInfo<Employee>>(restRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(new ListInfo<Employee> { Count = 0, Data = new List<Employee>() });
    }
}
