using System.Text.Json;
using BenchmarkDotNet.Attributes;
using R.Systems.Template.Benchmarks.Api.Web.Options;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using RestSharp;
using RestSharp.Authenticators;

namespace R.Systems.Template.Benchmarks.Api.Web.Employees;

public class GetEmployeesBenchmark
{
    private readonly string _endpointUrlPath = "/employees";
    private RestClient? _restClient;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _restClient = new RestClient(
            new RestClientOptions(ApiOptions.ApiBaseUrl)
            {
                ThrowOnDeserializationError = false,
                Authenticator = new JwtAuthenticator(ApiOptions.AccessTokenAzureAdB2C)
            }
        );
    }

    [Benchmark]
    public async Task<ListInfo<Employee>> GetEmployeesAsync()
    {
        int page = 2;
        int pageSize = 100;
        RestRequest restRequest = new(_endpointUrlPath);
        restRequest.AddQueryParameter(nameof(page), page);
        restRequest.AddQueryParameter(nameof(pageSize), pageSize);
        RestResponse<ListInfo<Employee>> response = await _restClient!.ExecuteAsync<ListInfo<Employee>>(restRequest);
        if (response.Data == null)
        {
            Console.WriteLine(JsonSerializer.Serialize(response));
            throw new InvalidOperationException("response.Data is null");
        }

        return response.Data;
    }
}
