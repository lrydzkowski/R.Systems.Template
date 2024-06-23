namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

internal interface IOptionsData
{
    Dictionary<string, string?> ConvertToInMemoryCollection();
}
