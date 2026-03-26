using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Weather.Services;

namespace Weather.Persistence
{
    /// <summary>
    /// Creates the database context at design time for Entity Framework Core tools.
    /// </summary>
    public class WeatherDbDesignTimeContextFactory : IDesignTimeDbContextFactory<WeatherDatabaseContext>
    {
        /// <inheritdoc />
        public WeatherDatabaseContext CreateDbContext(string[] args)
        {
            var configurationService = new ConfigurationService();

            IConfiguration configuration = configurationService.BuildConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<WeatherDatabaseContext>();
            configurationService.ConfigureDatabaseOptions(optionsBuilder);

            return new WeatherDatabaseContext(optionsBuilder.Options);
        }
    }
}