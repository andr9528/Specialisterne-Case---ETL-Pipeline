using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Abstraction.Enum;
using Weather.Model.Entity;
using Weather.Model.Extensions;
using Weather.Persistence.Core;
using Weather.Persistence.Core.Abstraction;

namespace Weather.Persistence.Configuration
{
    public class DsConfiguration : EntityConfiguration<Ds>
    {
        /// <inheritdoc />
        public DsConfiguration(DatabaseType type) : base(type)
        {
        }

        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Ds> builder)
        {
            base.Configure(builder);

            builder.ToTable("DS18B20");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("DS18B20_id");

            builder.Property(x => x.ReaderId).HasColumnName(StringExtensions.ToSnakeCase(nameof(Ds.ReaderId)));

            builder.Property(x => x.Location).HasColumnName(StringExtensions.ToSnakeCase(nameof(Ds.Location))).HasMaxLength(7)
                .HasConversion(x => StringExtensions.ToSnakeCase(x.ToString()),
                    x => Enum.Parse<Location>(x, ignoreCase: true));

            builder.Property(x => x.Temperature).HasColumnName(StringExtensions.ToSnakeCase(nameof(Ds.Temperature)))
                .HasColumnType("numeric(20,13)");

            builder.Property(x => x.ObservedAt).HasColumnName(StringExtensions.ToSnakeCase(nameof(Ds.ObservedAt)));

            builder.Property(x => x.PulledAt).HasColumnName(StringExtensions.ToSnakeCase(nameof(Ds.PulledAt)));

            builder.Ignore(x => x.TemperatureUnit);
        }
    }
}