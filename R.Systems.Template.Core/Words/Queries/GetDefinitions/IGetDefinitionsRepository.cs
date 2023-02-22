namespace R.Systems.Template.Core.Words.Queries.GetDefinitions;

public interface IGetDefinitionsRepository
{
    Task<List<Definition>> GetDefinitionsAsync(string word);
}
