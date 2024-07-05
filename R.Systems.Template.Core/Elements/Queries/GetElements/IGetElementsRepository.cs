using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Elements.Queries.GetElements;

public interface IGetElementsRepository
{
    Task<ListInfo<Element>> GetElementsAsync(ListParameters listParameters, CancellationToken cancellationToken);
}
