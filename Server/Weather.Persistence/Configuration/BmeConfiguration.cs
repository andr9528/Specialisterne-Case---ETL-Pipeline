using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Abstraction.Enum;
using Weather.Model.Entity;
using Weather.Model.Extensions;
using Weather.Persistence.Core;
using Weather.Persistence.Core.Abstraction;

namespace Weather.Persistence.Configuration
{
    public class BmeConfiguration : EntityConfiguration<Bme>
    {
        /// <inheritdoc />
        public BmeConfiguration(DatabaseType type) : base(type)
        {
        }

        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Bme> builder)
        {
            base.Configure(builder);

            builder.ToTable("BME280");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("BME280_id");

            builder.Property(x => x.ReaderId).HasColumnName(StringExtensions.ToSnakeCase(nameof(Bme.ReaderId)));

            builder.Property(x => x.Location).HasColumnName(StringExtensions.ToSnakeCase(nameof(Bme.Location)))
                .HasMaxLength(7).HasConversion(x => StringExtensions.ToSnakeCase(x.ToString()),
                    x => Enum.Parse<Location>(x, ignoreCase: true));

            builder.Property(x => x.Humidity).HasColumnName(StringExtensions.ToSnakeCase(nameof(Bme.Humidity)))
                .HasColumnType("numeric(20,13)");

            builder.Property(x => x.Pressure).HasColumnName(StringExtensions.ToSnakeCase(nameof(Bme.Pressure)))
                .HasColumnType("numeric(20,13)");

            builder.Property(x => x.Temperature).HasColumnName(StringExtensions.ToSnakeCase(nameof(Bme.Temperature)))
                .HasColumnType("numeric(20,13)");

            builder.Property(x => x.ObservedAt).HasColumnName(StringExtensions.ToSnakeCase(nameof(Bme.ObservedAt)));

            builder.Property(x => x.PulledAt).HasColumnName(StringExtensions.ToSnakeCase(nameof(Bme.PulledAt)));

            builder.Ignore(x => x.HumidityUnit);
            builder.Ignore(x => x.PressureUnit);
            builder.Ignore(x => x.TemperatureUnit);
        }
    }
}