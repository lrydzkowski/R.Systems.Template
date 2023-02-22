using System.Net;
using Microsoft.Extensions.Options;
using R.Systems.Template.Infrastructure.Wordnik.Common.Models;
using R.Systems.Template.Infrastructure.Wordnik.Common.Options;
using RestSharp;

namespace R.Systems.Template.Infrastructure.Wordnik.Common.Api;

internal class WordApi
{
    private readonly string _sourceDictionaries = "ahd-5";

    public WordApi(IOptions<WordnikOptions> options)
    {
        WordnikOptions = options.Value;
        RestClient = new RestClient(WordnikOptions.ApiBaseUrl);
        RestClient.Options.ThrowOnDeserializationError = false;
    }

    private WordnikOptions WordnikOptions { get; }
    private RestClient RestClient { get; init; }

    public async Task<List<DefinitionDto>> GetDefinitionsAsync(string word)
    {
        RestRequest restRequest = new(WordnikOptions.DefinitionsUrl.Replace("{word}", word));
        restRequest.AddQueryParameter("limit", "10");
        restRequest.AddQueryParameter("includeRelated", false);
        restRequest.AddQueryParameter("sourceDictionaries", _sourceDictionaries);
        restRequest.AddQueryParameter("useCanonical", false);
        restRequest.AddQueryParameter("includeTags", false);
        restRequest.AddQueryParameter("api_key", WordnikOptions.ApiKey);

        RestResponse<List<DefinitionDto>?> response = await RestClient.ExecuteAsync<List<DefinitionDto>?>(restRequest);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new List<DefinitionDto>();
        }

        HandleUnexpectedError(response);

        return response.Data!;
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
