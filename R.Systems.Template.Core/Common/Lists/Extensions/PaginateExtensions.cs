namespace R.Systems.Template.Core.Common.Lists.Extensions;

public static class PaginateExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, Pagination pagination)
    {
        if (pagination.PageSize > -1)
        {
            int page = pagination.Page > 0 ? pagination.Page : 1;
            query = query.Skip((page - 1) * pagination.PageSize).Take(pagination.PageSize);
        }

        return query;
    }
}
