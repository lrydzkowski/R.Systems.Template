namespace R.Systems.Template.Persistence.Db.DataGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            new AppRunnerFactory().Create().Run(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error has occurred!");
            Console.WriteLine(ex);
        }
    }
}
