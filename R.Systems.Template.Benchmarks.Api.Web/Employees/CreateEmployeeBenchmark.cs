using System.Text.Json;
using BenchmarkDotNet.Attributes;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Benchmarks.Api.Web.Options;
using R.Systems.Template.Core.Common.Domain;
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
    public async Task<Employee> CreateEmployeeAsync()
    {
        CreateEmployeeRequest request = new()
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            CompanyId = 10
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
}
