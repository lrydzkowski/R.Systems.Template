﻿using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Companies.Queries.GetCompanies;

public class GetCompaniesProtectedTests
{
    private readonly string _endpointUrlPath = "/companies-protected";

    [Fact]
    public async Task GetCompanies_ShouldReturn401_WhenAccessTokenIsNotPresent()
    {
        RestClient restClient = new WebApiFactory<Program>().CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Company>> response = await restClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetCompanies_ShouldReturnCompanies_WhenCompaniesExist()
    {
        List<Company> expectedCompanies = CompaniesSampleData.Data
            .Where(x => x.Value.Id != null)
            .Select(
                x => new Company
                {
                    CompanyId = (int)x.Value.Id!,
                    Name = x.Value.Name,
                    Employees = EmployeesSampleData.Data.Where(y => y.CompanyId == x.Value.Id && y.Id != null)
                        .Select(
                            y => new Employee
                            {
                                EmployeeId = (int)y.Id!,
                                FirstName = y.FirstName,
                                LastName = y.LastName
                            }
                        )
                        .ToList()
                }
            )
            .ToList();
        RestClient restClient = new WebApiFactory<Program>().WithoutAuthentication().CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Company>> response = await restClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedCompanies);
    }
}