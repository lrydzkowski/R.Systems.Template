namespace R.Systems.Template.Core.Common.Lists;

public class FieldInfo
{
    public string FieldName { get; init; } = "";

    public bool DefaultSorting { get; init; }

    public bool UseInSorting { get; init; } = true;

    public bool UseInFiltering { get; init; } = true;

    public bool AlwaysPresent { get; init; }
}
