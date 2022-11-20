﻿using FluentAssertions;
using R.Systems.Template.Tests.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Integration.Common.WebApplication;
using R.Systems.Template.WebApi.Api;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.App.Queries.GetAppInfo;

[Collection(QueryTestsCollection.CollectionName)]
public class GetAppInfoTests
{
    public GetAppInfoTests(WebApiFactory webApiFactory)
    {
        RestClient = webApiFactory.CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetAppInfo_ShouldReturnCorrectVersion_WhenCorrectDataIsPassed()
    {
        string expectedAppName = AppNameService.GetWebApiName();
        string semVerRegex = new SemVerRegex().Get();
        RestRequest request = new("/");

        RestResponse<GetAppInfoResponse> response = await RestClient.ExecuteAsync<GetAppInfoResponse>(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data?.AppName.Should().Be(expectedAppName);
        response.Data?.AppVersion.Should().MatchRegex(semVerRegex);
    }
}
