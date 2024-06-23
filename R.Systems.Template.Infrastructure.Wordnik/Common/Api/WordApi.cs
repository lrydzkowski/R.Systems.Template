using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Caching;
using Polly.Retry;
using Polly.Wrap;
using R.Systems.Template.Infrastructure.Wordnik.Common.Models;
using R.Systems.Template.Infrastructure.Wordnik.Common.Options;
using RestSharp;

namespace R.Systems.Template.Infrastructure.Wordnik.Common.Api;

internal class WordApi
{
    public const int RetryCount = 3;

    private readonly IAsyncCacheProvider _asyncCacheProvider;
    private readonly ILogger<WordApi> _logger;
    private readonly RestClient _restClient;
    private readonly string _sourceDictionaries = "ahd-5";
    private readonly WordnikOptions _wordnikOptions;

    public WordApi(ILogger<WordApi> logger, IAsyncCacheProvider asyncCacheProvider, IOptions<WordnikOptions> options)
    {
        _logger = logger;
        _asyncCacheProvider = asyncCacheProvider;
        _wordnikOptions = options.Value;
        _restClient = new RestClient(
            new RestClientOptions(_wordnikOptions.ApiBaseUrl) { ThrowOnDeserializationError = false }
        );
    }

    public async Task<List<DefinitionDto>> GetDefinitionsAsync(string word, CancellationToken cancellationToken)
    {
        RestRequest restRequest = new(_wordnikOptions.DefinitionsUrl.Replace("{word}", word));
        restRequest.AddQueryParameter("limit", "10");
        restRequest.AddQueryParameter("includeRelated", false);
        restRequest.AddQueryParameter("sourceDictionaries", _sourceDictionaries);
        restRequest.AddQueryParameter("useCanonical", false);
        restRequest.AddQueryParameter("includeTags", false);
        restRequest.AddQueryParameter("api_key", _wordnikOptions.ApiKey);
        RestResponse<List<DefinitionDto>?> response = await ExecuteWithPoliciesAsync(
            restRequest,
            word,
            cancellationToken
        );
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new List<DefinitionDto>();
        }

        HandleUnexpectedError(response);
        return response.Data!;
    }

    public async Task<string?> GetRandomWordAsync(CancellationToken cancellationToken)
    {
        RestRequest restRequest = new(_wordnikOptions.RandomWordUrl);
        restRequest.AddQueryParameter("api_key", _wordnikOptions.ApiKey);
        RestResponse<RandomWordDto> response =
            await _restClient.ExecuteAsync<RandomWordDto>(restRequest, cancellationToken);
        HandleUnexpectedError(response);
        return response.Data?.Word;
    }

    private async Task<RestResponse<List<DefinitionDto>?>> ExecuteWithPoliciesAsync(
        RestRequest restRequest,
        string word,
        CancellationToken cancellationToken
    )
    {
        AsyncPolicyWrap<RestResponse<List<DefinitionDto>?>> policyWrap = Policy.WrapAsync(
            DefineCachePolicy(),
            DefineRetryPolicy()
        );
        return await policyWrap.ExecuteAsync(
            async (_, c) => await _restClient.ExecuteAsync<List<DefinitionDto>?>(restRequest, c),
            new Context($"definitions_{word}"),
            cancellationToken
        );
    }

    private AsyncCachePolicy<RestResponse<List<DefinitionDto>?>> DefineCachePolicy()
    {
        return Policy.CacheAsync<RestResponse<List<DefinitionDto>?>>(_asyncCacheProvider, TimeSpan.FromHours(24));
    }

    private AsyncRetryPolicy<RestResponse<List<DefinitionDto>?>> DefineRetryPolicy()
    {
        return Policy.HandleResult<RestResponse<List<DefinitionDto>?>>(x => !x.IsSuccessful)
            .WaitAndRetryAsync(
                RetryCount,
                _ => TimeSpan.FromSeconds(3),
                (response, timeSpan, retryCount, _) =>
                {
                    _logger.LogWarning(
                        "Wordnik API request failed. HttpStatusCode = {StatusCode}. Waiting {TimeSpan} seconds before retry. Number attempt {RetryCount}. Uri = {Uri}. RequestResponse = {RequestResponse}.",
                        response.Result.StatusCode,
                        timeSpan,
                        retryCount,
                        response.Result.ResponseUri,
                        response.Result.Content
                    );
                }
            );
    }

    private void HandleUnexpectedError(RestResponse response)
    {
        if (response.IsSuccessful)
        {
            return;
        }

        throw new WordnikApiException(
            $"Unexpected error has occurred in communication with Wordnik API. Error message: '{response.ErrorMessage}'",
            response.ErrorException
        );
    }
}
