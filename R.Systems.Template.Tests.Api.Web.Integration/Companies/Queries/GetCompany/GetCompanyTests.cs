﻿using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Api.Web.Integration.Companies.Queries.GetCompany;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetCompanyTests
{
    private readonly string _endpointUrlPath = "/companies";

    public GetCompanyTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        RestClient = webApiFactory.CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetCompany_ShouldReturnCompany_WhenCompanyExists()
    {
        int companyId = IdGenerator.GetCompanyId(1);
        int employeeId = IdGenerator.GetEmployeeId(1);
        Company expectedCompany = new()
        {
            CompanyId = companyId,
            Name = "Meta",
            Employees = new List<Employee>()
            {
                new()
                {
                    EmployeeId = employeeId,
                    FirstName = "John",
                    LastName = "Doe"
                }
            }
        };
        RestRequest restRequest = new($"{_endpointUrlPath}/{companyId}");

        RestResponse<Company> response = await RestClient.ExecuteAsync<Company>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedCompany);
    }

    [Fact]
    public async Task GetCompany_ShouldReturn404_WhenCompanyNotExist()
    {
        int companyId = 10;
        RestRequest restRequest = new($"{_endpointUrlPath}/{companyId}");

        RestResponse<ErrorInfo> response = await RestClient.ExecuteAsync<ErrorInfo>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Data.Should()
            .BeEquivalentTo(
                new ErrorInfo
                {
                    PropertyName = "Company",
                    ErrorMessage = "Company doesn't exist.",
                    ErrorCode = "NotExist"
                },
                options => options.Including(x => x.PropertyName).Including(x => x.ErrorMessage)
            );
    }
}