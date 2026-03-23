using Microsoft.EntityFrameworkCore;
using Weather.Model.Entity;
using Weather.Persistence.Configuration;
using Weather.Persistence.Core;
using Weather.Persistence.Core.Abstraction;
using Weather.Persistence.Services;

namespace Weather.Persistence
{
    public class WeatherDatabaseContext : BaseDatabaseContext<WeatherDatabaseContext>
    {
        /// <inheritdoc />
        public WeatherDatabaseContext(DbContextOptions<WeatherDatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<Bme> Bme { get; set; }
        public virtual DbSet<Dmi> Dmi { get; set; }
        public virtual DbSet<Ds> Ds { get; set; }
        public virtual DbSet<Scd> Scd { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var databaseType =
                DatabaseType.POSTGRESQL; // This can be made dynamic based on configuration or environment variables.

            modelBuilder.ApplyConfiguration(new BmeConfiguration(databaseType));
            modelBuilder.ApplyConfiguration(new DmiConfiguration(databaseType));
            modelBuilder.ApplyConfiguration(new DsConfiguration(databaseType));
            modelBuilder.ApplyConfiguration(new ScdConfiguration(databaseType));
        }
    }
}
