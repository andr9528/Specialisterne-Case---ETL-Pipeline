using Microsoft.Extensions.DependencyInjection;

namespace Weather.Abstraction.Interfaces.Startup
{
    public interface IServiceStartupModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}