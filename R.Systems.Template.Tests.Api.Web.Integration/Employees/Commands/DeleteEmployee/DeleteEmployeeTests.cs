using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Infrastructure.Db.Common.Configurations;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Commands.DeleteEmployee;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class DeleteEmployeeTests
{
    private readonly string _endpointUrlPath = "/employees";

    public DeleteEmployeeTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        RestClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task DeleteEmployee_ShouldReturnValidationError_WhenEmployeeNotExist()
    {
        int employeeId = int.MaxValue;
        List<ValidationFailure> expectedValidationFailures = new()
        {
            new ValidationFailure
            {
                PropertyName = "Employee",
                ErrorMessage = $"Employee with the given id doesn't exist ('{employeeId}').",
                AttemptedValue = employeeId,
                ErrorCode = "NotExist"
            }
        };
        string url = $"{_endpointUrlPath}/{employeeId}";
        RestRequest deleteRequest = new(url, Method.Delete);

        RestResponse<List<ValidationFailure>> deleteResponse =
            await RestClient.ExecuteAsync<List<ValidationFailure>>(deleteRequest);

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        deleteResponse.Data.Should().NotBeNull();
        deleteResponse.Data.Should()
            .BeEquivalentTo(
                expectedValidationFailures,
                options => options.WithStrictOrdering()
            );
    }

    [Fact]
    public async Task DeleteEmployee_ShouldDeleteEmployee_WhenEmployeeExists()
    {
        CreateEmployeeCommand createEmployeeCommand = new()
        {
            CompanyId = CompanyConfiguration.FirstAvailableId,
            FirstName = "John",
            LastName = "Smith"
        };
        RestRequest createRequest = new(_endpointUrlPath, Method.Post);
        createRequest.AddJsonBody(createEmployeeCommand);
        RestResponse<Employee> createEmployeeResponse = await RestClient.ExecuteAsync<Employee>(createRequest);

        string url = $"{_endpointUrlPath}/{createEmployeeResponse.Data?.EmployeeId}";

        RestRequest getRequest = new(url);
        RestResponse<Employee> getResponse = await RestClient.ExecuteAsync<Employee>(getRequest);

        RestRequest deleteRequest = new(url, Method.Delete);
        RestResponse deleteResponse = await RestClient.ExecuteAsync(deleteRequest);

        RestRequest getRequestAfterDelete = new(url);
        RestResponse getResponseAfterDelete = await RestClient.ExecuteAsync(getRequestAfterDelete);

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data?.Should()
            .BeEquivalentTo(
                new Employee
                {
                    FirstName = createEmployeeCommand.FirstName,
                    LastName = createEmployeeCommand.LastName,
                    CompanyId = createEmployeeCommand.CompanyId
                },
                options => options.Excluding(x => x.EmployeeId)
            );
        getResponseAfterDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
