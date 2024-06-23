using System.Net;
using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Api.Web.Integration.Companies.Commands.CreateCompany;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class CreateCompanyTests
{
    private readonly string _endpointUrlPath = "/companies";

    private readonly ITestOutputHelper _output;
    private readonly RestClient _restClient;

    public CreateCompanyTests(ITestOutputHelper output, WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        _output = output;
        _restClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

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
        _output.WriteLine("Parameters set with id = {0}", id);
        RestRequest restRequest = new RestRequest(_endpointUrlPath, Method.Post).AddJsonBody(command);
        RestResponse<List<ValidationFailure>> response =
            await _restClient.ExecuteAsync<List<ValidationFailure>>(restRequest);
        response.StatusCode.Should().Be(expectedHttpStatus);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(validationFailures, options => options.WithStrictOrdering());
    }

    [Theory]
    [MemberData(nameof(CreateCompanyCorrectDataBuilder.Build), MemberType = typeof(CreateCompanyCorrectDataBuilder))]
    public async Task CreateCompany_ShouldCreateCompany_WhenDataIsCorrect(int id, CreateCompanyCommand command)
    {
        _output.WriteLine("Parameters set with id = {0}", id);
        RestRequest createRequest = new RestRequest(_endpointUrlPath, Method.Post).AddJsonBody(command);
        RestResponse<Company> createResponse = await _restClient.ExecuteAsync<Company>(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        createResponse.Data.Should().NotBeNull();
        createResponse.Data.Should()
            .BeEquivalentTo(new Company { Name = command.Name! }, options => options.Excluding(ctx => ctx.CompanyId));
        createResponse.Headers.Should().NotBeNullOrEmpty();
        createResponse.Headers.Should().Contain(x => x.Name == "Location");
        Company company = createResponse.Data!;
        string? companyUrl = createResponse.Headers!.First(x => x.Name == "Location").Value?.ToString();
        companyUrl.Should().NotBeNull();
        RestRequest getRequest = new(companyUrl);
        RestResponse<Company> getResponse = await _restClient.ExecuteAsync<Company>(getRequest);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should().BeEquivalentTo(company);
    }
}
