using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Companies.Commands.CreateCompany;

public class CreateCompanyTests : IClassFixture<WebApiFactory<Program>>
{
    private readonly string _endpointUrlPath = "/companies";

    public CreateCompanyTests(WebApiFactory<Program> webApiFactory)
    {
        RestClient = new RestClient(webApiFactory.CreateClient());
    }

    private RestClient RestClient { get; }

    [Theory]
    [MemberData(
        nameof(CreateCompanyIncorrectDataBuilder.Build),
        MemberType = typeof(CreateCompanyIncorrectDataBuilder)
    )]
    public async Task CreateCompany_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        CreateCompanyCommand command,
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
    [MemberData(nameof(CreateCompanyCorrectDataBuilder.Build), MemberType = typeof(CreateCompanyCorrectDataBuilder))]
    public async Task CreateCompany_ShouldCreateCompany_WhenDataIsCorrect(int id, CreateCompanyCommand command)
    {
        var createRequest = new RestRequest(_endpointUrlPath, Method.Post).AddJsonBody(command);

        RestResponse<Company> createResponse = await RestClient.ExecuteAsync<Company>(createRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        createResponse.Data.Should().NotBeNull();
        createResponse.Data.Should().BeEquivalentTo(
            new Company
            {
                Name = command.Name!,
                Employees = new List<Employee>()
            },
            options => options.Excluding(ctx => ctx.CompanyId)
        );
        createResponse.Headers.Should().NotBeNullOrEmpty();
        createResponse.Headers.Should().Contain(x => x.Name == "Location");

        Company company = createResponse.Data!;
        string? companyUrl = createResponse.Headers!.First(x => x.Name == "Location").Value?.ToString();
        companyUrl.Should().NotBeNull();

        var getRequest = new RestRequest(companyUrl);

        RestResponse<Company> getResponse = await RestClient.ExecuteAsync<Company>(getRequest);

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should().BeEquivalentTo(company);
    }
}
