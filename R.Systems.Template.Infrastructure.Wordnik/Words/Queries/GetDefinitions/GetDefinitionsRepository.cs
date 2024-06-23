using R.Systems.Template.Core.Words.Queries.GetDefinitions;
using R.Systems.Template.Infrastructure.Wordnik.Common.Api;
using R.Systems.Template.Infrastructure.Wordnik.Common.Models;

namespace R.Systems.Template.Infrastructure.Wordnik.Words.Queries.GetDefinitions;

internal class GetDefinitionsRepository : IGetDefinitionsRepository
{
    private readonly WordApi _wordApi;

    public GetDefinitionsRepository(WordApi wordApi)
    {
        _wordApi = wordApi;
    }

    public async Task<List<Definition>> GetDefinitionsAsync(string word, CancellationToken cancellationToken)
    {
        DefinitionDtoMapper mapper = new();
        List<DefinitionDto> definitionsDto = await _wordApi.GetDefinitionsAsync(word, cancellationToken);
        return mapper.ToDefinitions(definitionsDto);
    }
}
