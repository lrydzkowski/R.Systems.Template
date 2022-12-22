using CommandDotNet;

namespace R.Systems.Template.Persistence.Db.DataGenerator;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            AppRunner runner = await new AppRunnerFactory().CreateAsync();
            await runner.RunAsync(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error has occurred!");
            Console.WriteLine(ex);
        }
    }
}
