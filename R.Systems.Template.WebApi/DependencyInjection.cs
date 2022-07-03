namespace R.Systems.Template.WebApi;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
