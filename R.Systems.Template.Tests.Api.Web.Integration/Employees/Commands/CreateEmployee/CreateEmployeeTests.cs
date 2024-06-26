using System.Net;
using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Commands.CreateEmployee;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class CreateEmployeeTests
{
    private readonly string _endpointUrlPath = "/employees";

    private readonly ITestOutputHelper _output;
    private readonly RestClient _restClient;

    public CreateEmployeeTests(ITestOutputHelper output, WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        _output = output;
        _restClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

    [Theory]
    [MemberData(
        nameof(CreateEmployeeIncorrectDataBuilder.Build),
        MemberType = typeof(CreateEmployeeIncorrectDataBuilder)
    )]
    public async Task CreateEmployee_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        CreateEmployeeCommand command,
        HttpStatusCode expectedHttpStatus,
        IEnumerable<ValidationFailure> validationFailures
    )
    {
        _output.WriteLine("Parameters set with id = {0}", id);
        RestRequest restRequest = new RestRequest(_endpointUrlPath, Method.Post).AddJsonBody(command);
        RestResponse<List<ValidationFailure>> response =
            await _restClient.ExecuteAsync<List<ValidationFailure>>(restRequest);
        response.StatusCode.Should().Be(expectedHttpStatus);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(validationFailures, options => options.WithStrictOrdering());
    }

    [Theory]
    [MemberData(nameof(CreateEmployeeCorrectDataBuilder.Build), MemberType = typeof(CreateEmployeeCorrectDataBuilder))]
    public async Task CreateEmployee_ShouldCreateEmployee_WhenDataIsCorrect(int id, CreateEmployeeCommand command)
    {
        _output.WriteLine("Parameters set with id = {0}", id);
        RestRequest createRequest = new RestRequest(_endpointUrlPath, Method.Post).AddJsonBody(command);
        RestResponse<Employee> createResponse = await _restClient.ExecuteAsync<Employee>(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        createResponse.Data.Should().NotBeNull();
        createResponse.Data.Should()
            .BeEquivalentTo(
                new Employee
                    { FirstName = command.FirstName!, LastName = command.LastName!, CompanyId = command.CompanyId! },
                options => options.Excluding(x => x.EmployeeId)
            );
        createResponse.Headers.Should().NotBeNullOrEmpty();
        createResponse.Headers.Should().Contain(x => x.Name == "Location");
        Employee employee = createResponse.Data!;
        string? employeeUrl = createResponse.Headers!.First(x => x.Name == "Location").Value?.ToString();
        employeeUrl.Should().NotBeNull();
        RestRequest getRequest = new(employeeUrl);
        RestResponse<Employee> getResponse = await _restClient.ExecuteAsync<Employee>(getRequest);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should().BeEquivalentTo(employee);
    }
}
