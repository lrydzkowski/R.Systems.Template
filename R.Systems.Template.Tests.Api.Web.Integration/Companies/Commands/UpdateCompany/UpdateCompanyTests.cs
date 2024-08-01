using System.Net;
using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Api.Web.Integration.Companies.Commands.UpdateCompany;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class UpdateCompanyTests
{
    private readonly string _endpointUrlPath = "/companies";

    private readonly ITestOutputHelper _output;
    private readonly RestClient _restClient;

    public UpdateCompanyTests(ITestOutputHelper output, WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        _output = output;
        _restClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

    [Theory]
    [MemberData(
        nameof(UpdateCompanyIncorrectDataBuilder.Build),
        MemberType = typeof(UpdateCompanyIncorrectDataBuilder)
    )]
    public async Task UpdateCompany_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        string companyId,
        UpdateCompanyRequest request,
        HttpStatusCode expectedHttpStatus,
        IEnumerable<ValidationFailure> validationFailures
    )
    {
        _output.WriteLine("Parameters set with id = {0}", id);
        string url = $"{_endpointUrlPath}/{companyId}";
        RestRequest restRequest = new RestRequest(url, Method.Put).AddJsonBody(request);
        RestResponse<List<ValidationFailure>> response =
            await _restClient.ExecuteAsync<List<ValidationFailure>>(restRequest);
        response.StatusCode.Should().Be(expectedHttpStatus);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(validationFailures, options => options.WithStrictOrdering());
    }

    [Theory]
    [MemberData(nameof(UpdateEmployeeCorrectDataBuilder.Build), MemberType = typeof(UpdateEmployeeCorrectDataBuilder))]
    public async Task UpdateCompany_ShouldUpdateCompany_WhenDataIsCorrect(
        int id,
        Guid companyId,
        UpdateCompanyRequest request
    )
    {
        _output.WriteLine("Parameters set with id = {0}", id);
        string url = $"{_endpointUrlPath}/{companyId}";
        RestRequest updateRequest = new RestRequest(url, Method.Put).AddJsonBody(request);
        RestResponse<Company> updateResponse = await _restClient.ExecuteAsync<Company>(updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Data.Should().NotBeNull();
        updateResponse.Data.Should().BeEquivalentTo(new Company { CompanyId = companyId, Name = request.Name! });
        Company company = updateResponse.Data!;
        RestRequest getRequest = new(url);
        RestResponse<Company> getResponse = await _restClient.ExecuteAsync<Company>(getRequest);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should().BeEquivalentTo(company);
    }
}
