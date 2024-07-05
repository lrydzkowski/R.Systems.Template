namespace R.Systems.Template.Core.Common.Lists;

public class FieldInfo
{
    public string FieldName { get; init; } = "";

    public bool DefaultSorting { get; init; }

    public bool UseInSorting { get; init; }

    public bool UseInFiltering { get; init; }

    public bool AlwaysPresent { get; init; }
}
