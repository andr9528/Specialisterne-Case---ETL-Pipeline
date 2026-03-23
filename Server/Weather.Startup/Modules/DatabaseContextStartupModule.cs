using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Weather.Abstraction.Interfaces.Startup;
using Weather.Persistence.Core;

namespace Weather.Startup.Modules
{
    public class DatabaseContextStartupModule<TContext> : IServiceStartupModule
        where TContext : BaseDatabaseContext<TContext>
    {
        public delegate void SetupOptionsDelegate(DbContextOptionsBuilder options);

        private readonly SetupOptionsDelegate setupOptions;
        private readonly StartupHandling startupHandling;
        protected ILogger<DatabaseContextStartupModule<TContext>>? logger;

        public DatabaseContextStartupModule(SetupOptionsDelegate setup, StartupHandling startupHandling = StartupHandling.MIGRATE)
        {
            if (typeof(TContext) is { IsAbstract: true, })
                throw new ArgumentException($"Invalid type argument supplied to '{nameof(TContext)}'");

            setupOptions = setup ?? throw new ArgumentNullException(nameof(setup));
            this.startupHandling = startupHandling;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            logger = services.BuildServiceProvider().GetService<ILogger<DatabaseContextStartupModule<TContext>>>();

            services.AddDbContext<TContext>(options => setupOptions.Invoke(options));

            logger?.LogDebug("Completed Configuration of Database Services.");

            PerformStartupHandling(startupHandling, services);
        }

        private void PerformStartupHandling(StartupHandling handling, IServiceCollection services)
        {
            ServiceProvider? provider = services.BuildServiceProvider();
            using var context = provider.GetService<TContext>();

            switch (handling)
            {
                case StartupHandling.MIGRATE:
                    context?.Database.Migrate();
                    logger?.LogDebug("Completed Migration of Database.");
                    break;
                case StartupHandling.CREATE:
                    context?.Database.EnsureCreated();
                    logger?.LogDebug("Completed Ensure Creation of Database.");
                    break;
                case StartupHandling.CLEAR_CREATE:
                    context?.Database.EnsureDeleted();
                    context?.Database.EnsureCreated();
                    logger?.LogDebug("Completed Clearing and Creation of Database.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(handling), handling, null);
            }
        }
    }

    public enum StartupHandling
    {
        MIGRATE = 0,
        CREATE = 1,
        CLEAR_CREATE = 2,
    }
}
