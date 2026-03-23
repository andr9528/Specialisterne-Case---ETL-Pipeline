
namespace Weather.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var startup = new ApiStartup();

        var builder = WebApplication.CreateBuilder(args);

        startup.SetupServices(builder.Services);
        var app = builder.Build();
        startup.SetupApplication(app);

        app.Run();
    }
}
