using System.Text.Json;
using BenchmarkDotNet.Attributes;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Benchmarks.Api.Web.Options;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using RestSharp;
using RestSharp.Authenticators;

namespace R.Systems.Template.Benchmarks.Api.Web.Companies;

public class GetCompaniesBenchmark
{
    private readonly string _endpointUrlPath = "/companies";

    private RestClient? _restClient;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _restClient = new RestClient(
            new RestClientOptions(ApiOptions.ApiBaseUrl)
            {
                ThrowOnDeserializationError = false,
                Authenticator = new JwtAuthenticator(ApiOptions.AccessTokenAzureAd)
            }
        );
    }

    [Benchmark]
    public async Task<ListInfo<Company>> GetCompaniesPostgreSqlAsync()
    {
        int page = 4;
        int pageSize = 100;
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(page), page);
        restRequest.AddQueryParameter(nameof(pageSize), pageSize);

        RestResponse<ListInfo<Company>> response = await _restClient!.ExecuteAsync<ListInfo<Company>>(restRequest);
        if (response.Data == null)
        {
            Console.WriteLine(JsonSerializer.Serialize(response));

            throw new InvalidOperationException("response.Data is null");
        }

        return response.Data;
    }

    [Benchmark]
    public async Task<ListInfo<Company>> GetCompaniesSqlServerAsync()
    {
        int page = 4;
        int pageSize = 100;
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(page), page);
        restRequest.AddQueryParameter(nameof(pageSize), pageSize);
        restRequest.AddHeader(Headers.Version, Versions.V3);

        RestResponse<ListInfo<Company>> response = await _restClient!.ExecuteAsync<ListInfo<Company>>(restRequest);
        if (response.Data == null)
        {
            Console.WriteLine(JsonSerializer.Serialize(response));

            throw new InvalidOperationException("response.Data is null");
        }

        return response.Data;
    }
}
