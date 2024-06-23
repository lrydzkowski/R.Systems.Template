using MediatR;

namespace R.Systems.Template.Core.Words.Queries.GetDefinitions;

public class GetDefinitionsQuery : IRequest<GetDefinitionsResult>
{
    public string? Word { get; set; } = "";
}

public class GetDefinitionsResult
{
    public List<Definition> Definitions { get; init; } = new();
}

public class GetDefinitionsQueryHandler : IRequestHandler<GetDefinitionsQuery, GetDefinitionsResult>
{
    private readonly IGetDefinitionsRepository _getDefinitionsRepository;

    public GetDefinitionsQueryHandler(IGetDefinitionsRepository getDefinitionsRepository)
    {
        _getDefinitionsRepository = getDefinitionsRepository;
    }

    public async Task<GetDefinitionsResult> Handle(GetDefinitionsQuery query, CancellationToken cancellationToken)
    {
        string word = query.Word ?? "";
        List<Definition> definitions = await _getDefinitionsRepository.GetDefinitionsAsync(word, cancellationToken);
        return new GetDefinitionsResult
        {
            Definitions = definitions
        };
    }
}
