namespace Weather.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var startup = new ApiStartup();

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            startup.SetupServices(builder.Services);
            WebApplication app = builder.Build();
            startup.SetupApplication(app);

            app.Run();
        }
    }
}