using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Factories;
using RestSharp;
using System.Net;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Integration.Companies.Commands.CreateCompany;

public class CreateCompanyTests : IClassFixture<WebApiFactory>
{
    private readonly string _endpointUrlPath = "/companies";

    public CreateCompanyTests(ITestOutputHelper output, WebApiFactory webApiFactory)
    {
        Output = output;
        WebApiFactory = webApiFactory;
    }

    private ITestOutputHelper Output { get; }
    private WebApiFactory WebApiFactory { get; }

    [Theory]
    [MemberData(
        nameof(CreateCompanyIncorrectDataBuilder.Build),
        MemberType = typeof(CreateCompanyIncorrectDataBuilder)
    )]
    public async Task CreateCompany_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        CreateCompanyCommand command,
        HttpStatusCode expectedHttpStatus,
        IEnumerable<ValidationFailure> validationFailures
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        RestClient restClient = WebApiFactory.CreateRestClient();
        var restRequest = new RestRequest(_endpointUrlPath, Method.Post).AddJsonBody(command);

        RestResponse<List<ValidationFailure>> response = await restClient.ExecuteAsync<List<ValidationFailure>>(
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
        Output.WriteLine("Parameters set with id = {0}", id);

        RestClient restClient = WebApiFactory.CreateRestClient();
        var createRequest = new RestRequest(_endpointUrlPath, Method.Post).AddJsonBody(command);

        RestResponse<Company> createResponse = await restClient.ExecuteAsync<Company>(createRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        createResponse.Data.Should().NotBeNull();
        createResponse.Data.Should()
            .BeEquivalentTo(
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

        RestResponse<Company> getResponse = await restClient.ExecuteAsync<Company>(getRequest);

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should().BeEquivalentTo(company);
    }
}
