﻿using System.Net;
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

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Commands.UpdateEmployee;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class UpdateEmployeeTests
{
    private readonly string _endpointUrlPath = "/employees";

    public UpdateEmployeeTests(ITestOutputHelper output, WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        Output = output;
        RestClient = webApiFactory.WithoutAuthentication().CreateRestClient();
    }

    private ITestOutputHelper Output { get; }
    private RestClient RestClient { get; }

    [Theory]
    [MemberData(
        nameof(UpdateEmployeeIncorrectDataBuilder.Build),
        MemberType = typeof(UpdateEmployeeIncorrectDataBuilder)
    )]
    public async Task UpdateEmployee_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        int employeeId,
        UpdateEmployeeRequest request,
        HttpStatusCode expectedHttpStatus,
        IEnumerable<ValidationFailure> validationFailures
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        string url = $"{_endpointUrlPath}/{employeeId}";
        RestRequest restRequest = new RestRequest(url, Method.Put).AddJsonBody(request);

        RestResponse<List<ValidationFailure>> response = await RestClient.ExecuteAsync<List<ValidationFailure>>(
            restRequest
        );

        response.StatusCode.Should().Be(expectedHttpStatus);
        response.Data.Should().NotBeNull();
        response.Data.Should()
            .BeEquivalentTo(
                validationFailures,
                options => options.WithStrictOrdering()
            );
    }

    [Theory]
    [MemberData(nameof(UpdateEmployeeCorrectDataBuilder.Build), MemberType = typeof(UpdateEmployeeCorrectDataBuilder))]
    public async Task UpdateEmployee_ShouldUpdateCompany_WhenDataIsCorrect(
        int id,
        int employeeId,
        UpdateEmployeeRequest request
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        string url = $"{_endpointUrlPath}/{employeeId}";
        RestRequest updateRequest = new RestRequest(url, Method.Put).AddJsonBody(request);

        RestResponse<Employee> updateResponse = await RestClient.ExecuteAsync<Employee>(updateRequest);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Data.Should().NotBeNull();
        updateResponse.Data.Should()
            .BeEquivalentTo(
                new Employee
                {
                    EmployeeId = employeeId,
                    FirstName = request.FirstName!,
                    LastName = request.LastName!,
                    CompanyId = request.CompanyId
                }
            );

        Employee employee = updateResponse.Data!;

        RestRequest getRequest = new(url);

        RestResponse<Employee> getResponse = await RestClient.ExecuteAsync<Employee>(getRequest);

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Should().BeEquivalentTo(employee);
    }
}
