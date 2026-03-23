using Microsoft.EntityFrameworkCore;
using Weather.Model.Entity;
using Weather.Persistence.Core;
using Weather.Persistence.Core.Abstraction;

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
        }
    }
}
