using R.Systems.Template.Core.Words.Queries.GetDefinitions;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Infrastructure.Wordnik.Common.Models;

[Mapper]
internal partial class DefinitionDtoMapper
{
    public partial List<Definition> ToDefinitions(List<DefinitionDto> definitionDtos);

    private string MapToString(DefinitionExampleUsesDto exampleUsesDto)
    {
        return exampleUsesDto.Text;
    }
}
