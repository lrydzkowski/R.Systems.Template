using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Employees.Queries.GetEmployeesInCompany;

[Collection(QueryWithoutDataTestsCollection.CollectionName)]
public class GetEmployeesInCompanyWithoutDataTests
{
    public GetEmployeesInCompanyWithoutDataTests(WebApiFactory webApiFactory)
    {
        RestClient = webApiFactory.WithoutData().CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetEmployeesInCompany_ShouldReturnEmptyList_WhenEmployeesNotExist()
    {
        int companyId = IdGenerator.GetCompanyId(1);
        RestRequest restRequest = new($"/companies/{companyId}/employees");

        RestResponse<List<Employee>> response = await RestClient.ExecuteAsync<List<Employee>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(new List<Employee>());
    }
}
