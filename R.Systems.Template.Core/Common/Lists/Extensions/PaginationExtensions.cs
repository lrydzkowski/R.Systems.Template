namespace R.Systems.Template.Core.Common.Lists.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, Pagination pagination)
    {
        if (pagination.PageSize <= -1)
        {
            return query;
        }

        int page = pagination.Page;
        if (page < 1)
        {
            page = 1;
        }

        query = query.Skip((page - 1) * pagination.PageSize).Take(pagination.PageSize);

        return query;
    }
}
