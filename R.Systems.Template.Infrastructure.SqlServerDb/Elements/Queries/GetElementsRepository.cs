using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Elements.Queries.GetElements;

namespace R.Systems.Template.Infrastructure.SqlServerDb.Elements.Queries;

internal class GetElementsRepository : IGetElementsRepository
{
    private readonly AppDbContext _dbContext;

    public GetElementsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ListInfo<Element>> GetElementsAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Element> query = _dbContext.Elements.AsNoTracking()
            .Select(
                elementEntity => new Element
                {
                    Id = elementEntity.Id!,
                    Name = elementEntity.Name,
                    Description = elementEntity.Description,
                    Value = elementEntity.Value,
                    AdditionalValue = elementEntity.AdditionalValue,
                    BigValue = elementEntity.BigValue,
                    BigAdditionalValue = elementEntity.BigAdditionalValue,
                    Price = elementEntity.Price,
                    Discount = elementEntity.Discount,
                    CreationDate = elementEntity.CreationDate,
                    UpdateDate = elementEntity.UpdateDate,
                    CreationDateTime = elementEntity.CreationDateTime,
                    UpdateDateTime = elementEntity.UpdateDateTime,
                    IsNew = elementEntity.IsNew,
                    IsActive = elementEntity.IsActive
                }
            )
            .Sort(listParameters.Sorting, listParameters.Fields)
            .Filter(listParameters.Filters, listParameters.Fields);
        List<Element> elements = await query
            .Paginate(listParameters.Pagination)
            .Project(listParameters.Fields)
            .ToListAsync(cancellationToken);
        int count = await query.CountAsync(cancellationToken);

        return new ListInfo<Element>
        {
            Data = elements,
            Count = count
        };
    }
}
