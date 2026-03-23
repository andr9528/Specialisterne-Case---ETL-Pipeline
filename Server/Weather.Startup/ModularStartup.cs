using Microsoft.Extensions.DependencyInjection;
using Weather.Abstraction.Interfaces.Startup;

namespace Weather.Startup
{
    public class ModularStartup<TApplicationBuilder>
    {
        private readonly ICollection<IServiceStartupModule> serviceModules;
        private readonly ICollection<IApplicationStartupModule<TApplicationBuilder>> applicationModules;

        protected ModularStartup()
        {
            serviceModules = new List<IServiceStartupModule>();
            applicationModules = new List<IApplicationStartupModule<TApplicationBuilder>>();
        }

        public IServiceCollection Services { get; protected set; } = null!;
        public IServiceProvider ServiceProvider { get; protected set; } = null!;

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        protected virtual void ConfigureApplication(TApplicationBuilder app)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="module"></param>
        /// <exception cref="ArgumentException">
        /// If the provided module does not implement either <see cref="IServiceStartupModule"/> or <see cref="IApplicationStartupModule{TApplicationBuilder}"/>.
        /// Or if the module has already been registered - Fail Fast approach.
        /// </exception>
        protected void AddModule<TModule>(TModule module) where TModule : class
        {
            ArgumentNullException.ThrowIfNull(module);

            var wasRegistered = false;

            if (module is IServiceStartupModule serviceModule && !serviceModules.Contains(serviceModule))
            {
                serviceModules.Add(serviceModule);
                wasRegistered = true;
            }

            if (module is IApplicationStartupModule<TApplicationBuilder> applicationModule &&
                !applicationModules.Contains(applicationModule))
            {
                applicationModules.Add(applicationModule);
                wasRegistered = true;
            }

            if (!wasRegistered)
                throw new ArgumentException(
                    $"{typeof(TModule).Name} must implement {nameof(IServiceStartupModule)} " +
                    $"and/or {nameof(IApplicationStartupModule<TApplicationBuilder>)}.", nameof(module));
        }

        public void SetupServices(IServiceCollection? services = null)
        {
            Services = services ?? new ServiceCollection();

            ConfigureServices(Services);

            foreach (IServiceStartupModule module in serviceModules)
                module.ConfigureServices(Services);

            ServiceProvider = Services.BuildServiceProvider();
        }

        public TApplicationBuilder SetupApplication(TApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            ConfigureApplication(app);

            foreach (IApplicationStartupModule<TApplicationBuilder> module in applicationModules)
                module.ConfigureApplication(app);

            return app;
        }
    }
}
