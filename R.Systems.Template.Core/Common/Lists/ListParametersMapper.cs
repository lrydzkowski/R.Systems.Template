namespace R.Systems.Template.Core.Common.Lists;

public interface IListParametersMapper
{
    ListParameters Map(ListParametersDto listParametersDto, IReadOnlyList<FieldInfo> fields);
}

internal class ListParametersMapper
    : IListParametersMapper
{
    public ListParameters Map(ListParametersDto listParametersDto, IReadOnlyList<FieldInfo> fields)
    {
        return new ListParameters
        {
            Fields = PrepareFields(listParametersDto, fields),
            Pagination = new Pagination
            {
                Page = listParametersDto.Page,
                PageSize = listParametersDto.PageSize
            },
            Sorting = new Sorting
            {
                FieldName = listParametersDto.SortingFieldName,
                DefaultFieldName = fields.FirstOrDefault(x => x.DefaultSorting)?.FieldName ?? "",
                Order = MapToSortingOrder(listParametersDto.SortingOrder)
            },
            Filters = string.IsNullOrWhiteSpace(listParametersDto.SearchQuery)
                ? []
                : new List<SearchFilterGroup>
                {
                    new()
                    {
                        Operator = FilterGroupOperator.Or,
                        Filters = fields.Select(
                                field => new SearchFilter
                                    { FieldName = field.FieldName, Value = listParametersDto.SearchQuery }
                            )
                            .ToList()
                    }
                }
        };
    }

    private IReadOnlyList<FieldInfo> PrepareFields(ListParametersDto listParametersDto, IReadOnlyList<FieldInfo> fields)
    {
        if (listParametersDto.FieldsToReturn.Count == 0)
        {
            return fields;
        }

        List<FieldInfo> fieldsToReturn = [];
        foreach (FieldInfo field in fields)
        {
            if (!listParametersDto.FieldsToReturn.Contains(field.FieldName, StringComparer.InvariantCultureIgnoreCase)
                && !field.AlwaysPresent)
            {
                continue;
            }

            fieldsToReturn.Add(field);
        }

        return fieldsToReturn;
    }

    private SortingOrder MapToSortingOrder(string sortingOrder)
    {
        return sortingOrder switch
        {
            "desc" => SortingOrder.Descending,
            _ => SortingOrder.Ascending
        };
    }
}
