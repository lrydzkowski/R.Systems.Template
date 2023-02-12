using Microsoft.Extensions.Hosting;

namespace R.Systems.Template.Api.AzureFunctions;

public class Program
{
    public static void Main()
    {
        new FunctionHostBuilder().Build().Run();
    }
}
