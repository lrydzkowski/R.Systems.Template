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
    private readonly IReadOnlyList<FieldInfo> _fields =
    [
        new FieldInfo
        {
            FieldName = nameof(Element.Id),
            DefaultSorting = true,
            AlwaysPresent = true
        },
        new FieldInfo { FieldName = nameof(Element.Name) },
        new FieldInfo { FieldName = nameof(Element.Description) },
        new FieldInfo { FieldName = nameof(Element.Value) },
        new FieldInfo { FieldName = nameof(Element.AdditionalValue) },
        new FieldInfo { FieldName = nameof(Element.BigValue) },
        new FieldInfo { FieldName = nameof(Element.BigAdditionalValue) },
        new FieldInfo { FieldName = nameof(Element.Price) },
        new FieldInfo { FieldName = nameof(Element.Discount) },
        new FieldInfo { FieldName = nameof(Element.CreationDate) },
        new FieldInfo { FieldName = nameof(Element.UpdateDate) },
        new FieldInfo { FieldName = nameof(Element.CreationDateTime) },
        new FieldInfo { FieldName = nameof(Element.UpdateDateTime) },
        new FieldInfo { FieldName = nameof(Element.IsNew) },
        new FieldInfo { FieldName = nameof(Element.IsActive) }
    ];

    private readonly IListParametersMapper _listParametersMapper;

    private readonly IGetElementsRepository _repository;

    public GetElementsQueryHandler(
        IGetElementsRepository repository,
        IListParametersMapper listParametersMapper
    )
    {
        _repository = repository;
        _listParametersMapper = listParametersMapper;
    }

    public async Task<GetElementsResult> Handle(GetElementsQuery query, CancellationToken cancellationToken)
    {
        ListParameters listParameters = _listParametersMapper.Map(query.ListParametersDto, _fields);
        ListInfo<Element> elements = await _repository.GetElementsAsync(listParameters, cancellationToken);

        return new GetElementsResult
        {
            Elements = elements
        };
    }
}
