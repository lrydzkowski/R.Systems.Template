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
    public GetDefinitionsQueryHandler(IGetDefinitionsRepository getDefinitionsRepository)
    {
        GetDefinitionsRepository = getDefinitionsRepository;
    }

    private IGetDefinitionsRepository GetDefinitionsRepository { get; }

    public async Task<GetDefinitionsResult> Handle(GetDefinitionsQuery query, CancellationToken cancellationToken)
    {
        string word = query.Word ?? "";
        List<Definition> definitions =
            await GetDefinitionsRepository.GetDefinitionsAsync(word, cancellationToken);

        return new GetDefinitionsResult
        {
            Definitions = definitions
        };
    }
}
