using AutoMapper;
using R.Systems.Template.Core.Words.Queries.GetDefinitions;
using R.Systems.Template.Infrastructure.Wordnik.Common.Api;
using R.Systems.Template.Infrastructure.Wordnik.Common.Models;

namespace R.Systems.Template.Infrastructure.Wordnik.Words.Queries.GetDefinitions;

internal class GetDefinitionsRepository : IGetDefinitionsRepository
{
    public GetDefinitionsRepository(WordApi wordApi, IMapper mapper)
    {
        WordApi = wordApi;
        Mapper = mapper;
    }

    private WordApi WordApi { get; }
    private IMapper Mapper { get; }

    public async Task<List<Definition>> GetDefinitionsAsync(string word)
    {
        List<DefinitionDto> definitionsDto = await WordApi.GetDefinitionsAsync(word);

        return Mapper.Map<List<Definition>>(definitionsDto);
    }
}
