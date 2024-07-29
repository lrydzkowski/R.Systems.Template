using System.Text.Json;
using BenchmarkDotNet.Attributes;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Benchmarks.Api.Web.Options;
using R.Systems.Template.Core.Common.Domain;
using RestSharp;
using RestSharp.Authenticators;

namespace R.Systems.Template.Benchmarks.Api.Web.Companies;

public class CreateCompanyBenchmark
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
    public async Task<Company> CreateCompanyAsync()
    {
        CreateCompanyRequest request = new()
        {
            Name = Guid.NewGuid().ToString()
        };
        RestRequest restRequest = new(_endpointUrlPath, Method.Post);
        restRequest.AddJsonBody(request);

        RestResponse<Company> response = await _restClient!.ExecuteAsync<Company>(restRequest);
        if (response.Data == null)
        {
            Console.WriteLine(JsonSerializer.Serialize(response));

            throw new InvalidOperationException("response.Data is null");
        }

        return response.Data;
    }
}
