using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Api.AzureFunctions.Mappers;

[Mapper]
internal partial class GetAppInfoMapper
{
    public partial GetAppInfoResponse ToResponse(GetAppInfoResult result);
}
