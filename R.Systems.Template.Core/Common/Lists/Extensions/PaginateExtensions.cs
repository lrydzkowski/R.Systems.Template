namespace R.Systems.Template.Core.Common.Lists.Extensions;

public static class PaginateExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, Pagination pagination)
    {
        if (pagination.NumberOfRows > -1)
        {
            query = query.Skip(pagination.FirstIndex).Take(pagination.NumberOfRows);
        }

        return query;
    }
}
