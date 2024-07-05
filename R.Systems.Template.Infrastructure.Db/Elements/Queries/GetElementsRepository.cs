using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Elements.Queries.GetElements;

namespace R.Systems.Template.Infrastructure.Db.Elements.Queries;

internal class GetElementsRepository : IGetElementsRepository
{
    public Task<ListInfo<Element>> GetElementsAsync(ListParameters listParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
