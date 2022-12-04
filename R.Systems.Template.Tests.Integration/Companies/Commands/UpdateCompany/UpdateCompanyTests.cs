using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi.Api;
using RestSharp;
using System.Net;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Integration.Companies.Commands.UpdateCompany;

public class UpdateCompanyTests : IClassFixture<WebApiFactory>
{
    private readonly string _endpointUrlPath = "/companies";

    public UpdateCompanyTests(ITestOutputHelper output, WebApiFactory webApiFactory)
    {
        Output = output;
        WebApiFactory = webApiFactory;
    }

    private ITestOutputHelper Output { get; }
    private WebApiFactory WebApiFactory { get; }

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
        IEnumerable<ValidationFailure> validationFailures
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        string url = $"{_endpointUrlPath}/{companyId}";
        RestClient restClient = WebApiFactory.CreateRestClient();
        var restRequest = new RestRequest(url, Method.Put).AddJsonBody(request);

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
    [MemberData(nameof(UpdateEmployeeCorrectDataBuilder.Build), MemberType = typeof(UpdateEmployeeCorrectDataBuilder))]
    public async Task UpdateCompany_ShouldUpdateCompany_WhenDataIsCorrect(
        int id,
        int companyId,
        UpdateCompanyRequest request
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        string url = $"{_endpointUrlPath}/{companyId}";
        RestClient restClient = WebApiFactory.CreateRestClient();
        var updateRequest = new RestRequest(url, Method.Put).AddJsonBody(request);

        RestResponse<Company> updateResponse = await restClient.ExecuteAsync<Company>(updateRequest);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Data.Should().NotBeNull();
        updateResponse.Data.Should()
            .BeEquivalentTo(
                new Company
                {
                    CompanyId = companyId,
                    Name = request.Name!
                },
                options => options.Excluding(ctx => ctx.Employees)
            );

        Company company = updateResponse.Data!;

        var getRequest = new RestRequest(url);

        RestResponse<Company> getResponse = await restClient.ExecuteAsync<Company>(getRequest);

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should()
            .BeEquivalentTo(
                company,
                options => options.Excluding(ctx => ctx.Employees)
            );
    }
}
