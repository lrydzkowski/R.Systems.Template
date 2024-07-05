using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Elements.Queries.GetElements;

public class GetElementsQuery : IGetListQuery, IRequest<GetElementsResult>
{
    public ListParametersDto ListParametersDto { get; init; } = new();
}

public class GetElementsResult
{
    public ListInfo<Element> Elements { get; init; } = new();
}

public class GetElementsQueryHandler : IRequestHandler<GetElementsQuery, GetElementsResult>
{
    private readonly IGetElementsRepository _getElementsRepository;

    public GetElementsQueryHandler(IGetElementsRepository getElementsRepository)
    {
        _getElementsRepository = getElementsRepository;
    }

    public Task<GetElementsResult> Handle(GetElementsQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
