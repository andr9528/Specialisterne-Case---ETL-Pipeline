using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Model.Entity;
using Weather.Model.Extensions;
using Weather.Persistence.Core;
using Weather.Persistence.Core.Abstraction;

namespace Weather.Persistence.Configuration
{
    public class ScdConfiguration : EntityConfiguration<Scd>
    {
        /// <inheritdoc />
        public ScdConfiguration(DatabaseType type) : base(type)
        {
        }

        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Scd> builder)
        {
            base.Configure(builder);

            builder.ToTable("SCD41");

            builder.HasKey(x => x.Id);

            // Special case (non-standard naming)
            builder.Property(x => x.Id).HasColumnName("SCD41_id");

            builder.Property(x => x.ReaderId).HasColumnName(StringExtensions.ToSnakeCase(nameof(Scd.ReaderId)));

            builder.Property(x => x.CarbonDioxide).HasColumnName("co2");

            builder.Property(x => x.Humidity).HasColumnName(StringExtensions.ToSnakeCase(nameof(Scd.Humidity)))
                .HasColumnType("numeric(20,13)");

            builder.Property(x => x.Temperature).HasColumnName(StringExtensions.ToSnakeCase(nameof(Scd.Temperature)))
                .HasColumnType("numeric(20,13)");

            builder.Property(x => x.ObservedAt).HasColumnName(StringExtensions.ToSnakeCase(nameof(Scd.ObservedAt)));

            builder.Property(x => x.PulledAt).HasColumnName(StringExtensions.ToSnakeCase(nameof(Scd.PulledAt)));

            builder.Ignore(x => x.HumidityUnit);
            builder.Ignore(x => x.TemperatureUnit);
            builder.Ignore(x => x.CarbonDioxideUnit);
        }
    }
}