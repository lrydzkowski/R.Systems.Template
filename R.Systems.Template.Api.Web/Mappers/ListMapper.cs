using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Lists;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Api.Web.Mappers;

[Mapper]
public partial class ListMapper
{
    public partial ListParametersDto ToListParametersDto(ListRequest request);
}
