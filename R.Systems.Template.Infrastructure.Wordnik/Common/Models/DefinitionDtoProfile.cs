using AutoMapper;
using R.Systems.Template.Core.Words.Queries.GetDefinitions;

namespace R.Systems.Template.Infrastructure.Wordnik.Common.Models;

internal class DefinitionDtoProfile : Profile
{
    public DefinitionDtoProfile()
    {
        CreateMap<DefinitionDto, Definition>()
            .ForMember(
                dest => dest.ExampleUses,
                options => options.MapFrom(source => source.ExampleUses.Select(exampleUse => exampleUse.Text).ToList())
            );
    }
}
