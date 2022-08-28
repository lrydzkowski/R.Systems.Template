using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;
using R.Systems.Template.WebApi.Api;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Companies.Commands.UpdateCompany;

public class UpdateCompanyTests : IClassFixture<WebApiFactory<Program>>
{
    private readonly string _endpointUrlPath = "/companies";

    public UpdateCompanyTests(WebApiFactory<Program> webApiFactory)
    {
        RestClient = new RestClient(webApiFactory.CreateClient());
    }

    private RestClient RestClient { get; }

    [Theory]
    [MemberData(
        nameof(UpdateCompanyIncorrectDataBuilder.Build),
        MemberType = typeof(UpdateCompanyIncorrectDataBuilder)
    )]
    public async Task UpdateCompany_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        int companyId,
        UpdateCompanyRequest request,
        HttpStatusCode expectedHttpStatus,
        IEnumerable<ValidationFailure> validationFailures)
    {
        string url = $"{_endpointUrlPath}/{companyId}";
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
    [MemberData(nameof(UpdateCompanyCorrectDataBuilder.Build), MemberType = typeof(UpdateCompanyCorrectDataBuilder))]
    public async Task UpdateCompany_ShouldCreateCompany_WhenDataIsCorrect(
        int id,
        int companyId,
        UpdateCompanyRequest request
    )
    {
        string url = $"{_endpointUrlPath}/{companyId}";
        var createRequest = new RestRequest(url, Method.Put).AddJsonBody(request);

        RestResponse<Company> createResponse = await RestClient.ExecuteAsync<Company>(createRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        createResponse.Data.Should().NotBeNull();
        createResponse.Data.Should().BeEquivalentTo(
            new Company
            {
                CompanyId = companyId,
                Name = request.Name!
            },
            options => options.Excluding(ctx => ctx.Employees)
        );

        Company company = createResponse.Data!;

        var getRequest = new RestRequest(url);

        RestResponse<Company> getResponse = await RestClient.ExecuteAsync<Company>(getRequest);

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should().BeEquivalentTo(
            company,
            options => options.Excluding(ctx => ctx.Employees)
        );
    }
}
