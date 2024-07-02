using CommandDotNet;
using R.Systems.Template.Api.DataGeneratorCli.Services;

namespace R.Systems.Template.Api.DataGeneratorCli.Commands;

[Command("elements", Description = "Generate elements in database.")]
internal class GenerateElementsCommand
{
    private readonly ElementService _elementService;

    public GenerateElementsCommand(ElementService elementService)
    {
        _elementService = elementService;
    }

    [DefaultCommand]
    public async Task ExecuteAsync(
        [Option("elements-count")] int elementsCount = 1000
    )
    {
        await _elementService.CreateElementsAsync(elementsCount);
    }
}
