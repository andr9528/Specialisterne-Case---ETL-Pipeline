using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Abstraction.Enum;
using Weather.Model.Entity;
using Weather.Model.Extensions;
using Weather.Persistence.Core;
using Weather.Persistence.Core.Abstraction;

namespace Weather.Persistence.Configuration
{
    public class DmiConfiguration : EntityConfiguration<Dmi>
    {
        /// <inheritdoc />
        public DmiConfiguration(DatabaseType type) : base(type)
        {
        }

        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Dmi> builder)
        {
            base.Configure(builder);

            builder.ToTable("DMI");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("DMI_id");

            builder.Property(x => x.DmiId).HasColumnName(StringExtensions.ToSnakeCase(nameof(Dmi.DmiId)));

            builder.Property(x => x.ParameterId).HasColumnName(StringExtensions.ToSnakeCase(nameof(Dmi.ParameterId)))
                .HasMaxLength(50).HasConversion(x => StringExtensions.ToSnakeCase(x.ToString()),
                    x => Enum.Parse<DmiParameter>(x, ignoreCase: true));

            builder.Property(x => x.Value).HasColumnName(StringExtensions.ToSnakeCase(nameof(Dmi.Value)))
                .HasColumnType("double precision");

            builder.Property(x => x.ObservedAt).HasColumnName(StringExtensions.ToSnakeCase(nameof(Dmi.ObservedAt)));

            builder.Property(x => x.PulledAt).HasColumnName(StringExtensions.ToSnakeCase(nameof(Dmi.PulledAt)));

            builder.Property(x => x.StationId).HasColumnName(StringExtensions.ToSnakeCase(nameof(Dmi.StationId)));

            builder.Ignore(x => x.ValueUnit);
        }
    }
}