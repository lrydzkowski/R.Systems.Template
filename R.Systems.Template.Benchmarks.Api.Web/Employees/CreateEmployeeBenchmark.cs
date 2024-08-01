using System.Text.Json;
using BenchmarkDotNet.Attributes;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Benchmarks.Api.Web.Options;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using RestSharp;
using RestSharp.Authenticators;

namespace R.Systems.Template.Benchmarks.Api.Web.Employees;

public class CreateEmployeeBenchmark
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
    public async Task<Employee> CreateEmployeePostgreSqlAsync()
    {
        CreateEmployeeRequest request = new()
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            CompanyId = "31b04626-ed12-4d79-b3d6-1430a72000d5"
        };
        RestRequest restRequest = new(_endpointUrlPath, Method.Post);
        restRequest.AddJsonBody(request);

        RestResponse<Employee> response = await _restClient!.ExecuteAsync<Employee>(restRequest);
        if (response.Data == null)
        {
            Console.WriteLine(JsonSerializer.Serialize(response));

            throw new InvalidOperationException("response.Data is null");
        }

        return response.Data;
    }

    [Benchmark]
    public async Task<Employee> CreateEmployeeSqlServerAsync()
    {
        CreateEmployeeRequest request = new()
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            CompanyId = "9e27c3b4-bf21-4ffe-bdbb-919a2fc9e2cc"
        };
        RestRequest restRequest = new(_endpointUrlPath, Method.Post);
        restRequest.AddJsonBody(request);
        restRequest.AddHeader(Headers.Version, Versions.V3);

        RestResponse<Employee> response = await _restClient!.ExecuteAsync<Employee>(restRequest);
        if (response.Data == null)
        {
            Console.WriteLine(JsonSerializer.Serialize(response));

            throw new InvalidOperationException("response.Data is null");
        }

        return response.Data;
    }
}
