namespace R.Systems.Template.Tests.Integration.Common.Options;

internal interface IOptionsData
{
    Dictionary<string, string?> ConvertToInMemoryCollection();
}
