using FluentAssertions;
using FluentValidation.Results;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;
using System.Text.Json;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Assertion;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Api.Web.Integration.Companies.Commands.DeleteCompany;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class DeleteCompanyTests
{
    private readonly string _endpointUrlPath = "/companies";

    public DeleteCompanyTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        RestClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task DeleteCompany_ShouldReturnValidationError_WhenCompanyNotExist()
    {
        int companyId = int.MaxValue;
        List<ValidationFailure> expectedValidationFailures = new()
        {
            new ValidationFailure
            {
                PropertyName = "Company",
                ErrorMessage = $"Company with the given id doesn't exist ('{companyId}').",
                AttemptedValue = companyId,
                ErrorCode = "NotExist"
            }
        };
        string url = $"{_endpointUrlPath}/{companyId}";
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
    public async Task DeleteCompany_ShouldDeleteCompany_WhenCompanyExists()
    {
        CreateCompanyCommand createCompanyCommand = new()
        {
            Name = "Test Company"
        };
        RestRequest createRequest = new(_endpointUrlPath, Method.Post);
        createRequest.AddJsonBody(createCompanyCommand);
        RestResponse<Company> createCompanyResponse = await RestClient.ExecuteAsync<Company>(createRequest);

        string url = $"{_endpointUrlPath}/{createCompanyResponse.Data?.CompanyId}";

        RestRequest getRequest = new(url);
        RestResponse<Company> getResponse = await RestClient.ExecuteAsync<Company>(getRequest);

        RestRequest deleteRequest = new(url, Method.Delete);
        RestResponse deleteResponse = await RestClient.ExecuteAsync(deleteRequest);

        RestRequest getRequestAfterDelete = new(url);
        RestResponse getResponseAfterDelete = await RestClient.ExecuteAsync(getRequestAfterDelete);

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data?.Name.Should().Be(createCompanyCommand.Name);
        getResponseAfterDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
