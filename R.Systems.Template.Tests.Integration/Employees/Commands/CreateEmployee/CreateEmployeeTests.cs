using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Employees.Commands.CreateEmployee;

public class CreateEmployeeTests : IClassFixture<WebApiFactory<Program>>
{
    private readonly string _endpointUrlPath = "/employees";

    public CreateEmployeeTests(WebApiFactory<Program> webApiFactory)
    {
        RestClient = new RestClient(webApiFactory.CreateClient());
    }

    private RestClient RestClient { get; }

    [Theory]
    [MemberData(
        nameof(CreateEmployeeIncorrectDataBuilder.Build),
        MemberType = typeof(CreateEmployeeIncorrectDataBuilder)
    )]
    public async Task CreateEmployee_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        CreateEmployeeCommand command,
        HttpStatusCode expectedHttpStatus,
        IEnumerable<ValidationFailure> validationFailures)
    {
        var restRequest = new RestRequest(_endpointUrlPath, Method.Post).AddJsonBody(command);

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
    [MemberData(nameof(CreateEmployeeCorrectDataBuilder.Build), MemberType = typeof(CreateEmployeeCorrectDataBuilder))]
    public async Task CreateEmployee_ShouldCreateEmployee_WhenDataIsCorrect(int id, CreateEmployeeCommand command)
    {
        var createRequest = new RestRequest(_endpointUrlPath, Method.Post).AddJsonBody(command);

        RestResponse<Employee> createResponse = await RestClient.ExecuteAsync<Employee>(createRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        createResponse.Data.Should().NotBeNull();
        createResponse.Data.Should().BeEquivalentTo(
            new Employee
            {
                FirstName = command.FirstName!,
                LastName = command.LastName!,
                CompanyId = command.CompanyId!
            },
            options => options.Excluding(x => x.EmployeeId)
        );
        createResponse.Headers.Should().NotBeNullOrEmpty();
        createResponse.Headers.Should().Contain(x => x.Name == "Location");

        Employee employee = createResponse.Data!;
        string? employeeUrl = createResponse.Headers!.First(x => x.Name == "Location").Value?.ToString();
        employeeUrl.Should().NotBeNull();

        var getRequest = new RestRequest(employeeUrl);

        RestResponse<Employee> getResponse = await RestClient.ExecuteAsync<Employee>(getRequest);

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should().BeEquivalentTo(employee);
    }
}
