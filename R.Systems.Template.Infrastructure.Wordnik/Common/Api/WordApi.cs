using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Caching;
using Polly.Retry;
using Polly.Wrap;
using R.Systems.Template.Infrastructure.Wordnik.Common.Models;
using R.Systems.Template.Infrastructure.Wordnik.Common.Options;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Infrastructure.Wordnik.Common.Api;

internal class WordApi
{
    public const int RetryCount = 3;

    private readonly string _sourceDictionaries = "ahd-5";

    public WordApi(ILogger<WordApi> logger, IAsyncCacheProvider asyncCacheProvider, IOptions<WordnikOptions> options)
    {
        Logger = logger;
        AsyncCacheProvider = asyncCacheProvider;
        WordnikOptions = options.Value;
        RestClient = new RestClient(
            new RestClientOptions(WordnikOptions.ApiBaseUrl) { ThrowOnDeserializationError = false }
        );
    }

    private ILogger<WordApi> Logger { get; }
    private IAsyncCacheProvider AsyncCacheProvider { get; }
    private WordnikOptions WordnikOptions { get; }
    private RestClient RestClient { get; init; }

    public async Task<List<DefinitionDto>> GetDefinitionsAsync(string word, CancellationToken cancellationToken)
    {
        RestRequest restRequest = new(WordnikOptions.DefinitionsUrl.Replace("{word}", word));
        restRequest.AddQueryParameter("limit", "10");
        restRequest.AddQueryParameter("includeRelated", false);
        restRequest.AddQueryParameter("sourceDictionaries", _sourceDictionaries);
        restRequest.AddQueryParameter("useCanonical", false);
        restRequest.AddQueryParameter("includeTags", false);
        restRequest.AddQueryParameter("api_key", WordnikOptions.ApiKey);

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
        RestRequest restRequest = new(WordnikOptions.RandomWordUrl);
        restRequest.AddQueryParameter("api_key", WordnikOptions.ApiKey);

        RestResponse<RandomWordDto> response =
            await RestClient.ExecuteAsync<RandomWordDto>(restRequest, cancellationToken);

        HandleUnexpectedError(response);

        return response.Data?.Word;
    }

    private async Task<RestResponse<List<DefinitionDto>?>> ExecuteWithPoliciesAsync(
        RestRequest restRequest,
        string word,
        CancellationToken cancellationToken
    )
    {
        AsyncPolicyWrap<RestResponse<List<DefinitionDto>?>> policyWrap =
            Policy.WrapAsync(DefineCachePolicy(), DefineRetryPolicy());

        return await policyWrap.ExecuteAsync(
            async (_, c) => await RestClient.ExecuteAsync<List<DefinitionDto>?>(restRequest, c),
            new Context($"definitions_{word}"),
            cancellationToken
        );
    }

    private AsyncCachePolicy<RestResponse<List<DefinitionDto>?>> DefineCachePolicy()
    {
        return Policy.CacheAsync<RestResponse<List<DefinitionDto>?>>(
            AsyncCacheProvider,
            TimeSpan.FromHours(24)
        );
    }

    private AsyncRetryPolicy<RestResponse<List<DefinitionDto>?>> DefineRetryPolicy()
    {
        return Policy
            .HandleResult<RestResponse<List<DefinitionDto>?>>(x => !x.IsSuccessful)
            .WaitAndRetryAsync(
                RetryCount,
                _ => TimeSpan.FromSeconds(3),
                (response, timeSpan, retryCount, _) =>
                {
                    Logger.LogWarning(
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
