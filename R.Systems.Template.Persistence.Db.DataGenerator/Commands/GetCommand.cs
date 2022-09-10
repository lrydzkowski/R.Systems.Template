using CommandDotNet;

namespace R.Systems.Template.Persistence.Db.DataGenerator.Commands;

[Command(
    name: "get",
    Description = "Get records from database."
)]
internal class GetCommand
{
    [Subcommand]
    public GetCompaniesCommand? GetCompaniesCommand { get; set; }
}
