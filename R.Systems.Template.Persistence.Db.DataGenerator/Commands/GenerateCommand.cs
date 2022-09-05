using CommandDotNet;

namespace R.Systems.Template.Persistence.Db.DataGenerator.Commands;

[Command(
    name: "generate",
    Description = "Generate data in database."
)]
internal class GenerateCommand
{
    [Subcommand]
    public GenerateCompaniesCommand? GenerateCompaniesCommand { get; set; }
}
