using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Api.Web.Mappers;

[Mapper]
public partial class GetAppInfoMapper
{
    public partial GetAppInfoResponse ToResponse(GetAppInfoResult result);
}
