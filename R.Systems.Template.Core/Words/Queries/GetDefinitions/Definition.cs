namespace R.Systems.Template.Core.Words.Queries.GetDefinitions;

public class Definition
{
    public string Word { get; init; } = "";

    public string Text { get; init; } = "";

    public List<string> ExampleUses { get; init; } = new();
}
