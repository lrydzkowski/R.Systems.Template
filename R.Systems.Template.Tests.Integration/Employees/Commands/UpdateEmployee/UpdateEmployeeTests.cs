using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common;
using R.Systems.Template.Tests.Integration.Common.Db;
using R.Systems.Template.Tests.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Integration.Common.WebApplication;
using R.Systems.Template.WebApi.Api;
using RestSharp;
using System.Net;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Integration.Employees.Commands.UpdateEmployee;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class UpdateEmployeeTests
{
    private readonly string _endpointUrlPath = "/employees";

    public UpdateEmployeeTests(ITestOutputHelper output, WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        Output = output;
        RestClient = webApiFactory.CreateRestClient();
    }

    private ITestOutputHelper Output { get; }
    private RestClient RestClient { get; }

    [Theory]
    [MemberData(
        nameof(UpdateEmployeeIncorrectDataBuilder.Build),
        MemberType = typeof(UpdateEmployeeIncorrectDataBuilder)
    )]
    public async Task UpdateEmployee_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        int employeeId,
        UpdateEmployeeRequest request,
        HttpStatusCode expectedHttpStatus,
        IEnumerable<ValidationFailure> validationFailures
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        string url = $"{_endpointUrlPath}/{employeeId}";
        var restRequest = new RestRequest(url, Method.Put).AddJsonBody(request);

        RestResponse<List<ValidationFailure>> response = await RestClient.ExecuteAsync<List<ValidationFailure>>(
            restRequest
        );

        response.StatusCode.Should().Be(expectedHttpStatus);
        response.Data.Should().NotBeNull();
        response.Data.Should()
            .BeEquivalentTo(
                validationFailures,
                options => options.Including(x => x.PropertyName)
                    .Including(x => x.ErrorMessage)
                    .Including(x => x.ErrorCode)
            );
    }

    [Theory]
    [MemberData(nameof(UpdateEmployeeCorrectDataBuilder.Build), MemberType = typeof(UpdateEmployeeCorrectDataBuilder))]
    public async Task UpdateEmployee_ShouldUpdateCompany_WhenDataIsCorrect(
        int id,
        int employeeId,
        UpdateEmployeeRequest request
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        string url = $"{_endpointUrlPath}/{employeeId}";
        var updateRequest = new RestRequest(url, Method.Put).AddJsonBody(request);

        RestResponse<Employee> updateResponse = await RestClient.ExecuteAsync<Employee>(updateRequest);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Data.Should().NotBeNull();
        updateResponse.Data.Should()
            .BeEquivalentTo(
                new Employee
                {
                    EmployeeId = employeeId,
                    FirstName = request.FirstName!,
                    LastName = request.LastName!,
                    CompanyId = request.CompanyId
                }
            );

        Employee employee = updateResponse.Data!;

        var getRequest = new RestRequest(url);

        RestResponse<Employee> getResponse = await RestClient.ExecuteAsync<Employee>(getRequest);

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should().BeEquivalentTo(employee);
    }
}
