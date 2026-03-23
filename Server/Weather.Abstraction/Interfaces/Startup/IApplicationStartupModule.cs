namespace Weather.Abstraction.Interfaces.Startup
{
    public interface IApplicationStartupModule<TApplicationBuilder>
    {
        void ConfigureApplication(TApplicationBuilder app);
    }
}