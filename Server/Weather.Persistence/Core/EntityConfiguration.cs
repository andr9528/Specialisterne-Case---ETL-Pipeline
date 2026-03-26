using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Persistence.Core.Abstraction;

namespace Weather.Persistence.Core
{
    public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DatabaseType type;

        protected EntityConfiguration(DatabaseType type)
        {
            this.type = type;
        }

        /// <inheritdoc />
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Ignore(x => x.CreatedDateTime);
            builder.Ignore(x => x.UpdatedDateTime);
            builder.Ignore(x => x.Version);

            return;

            switch (this.type)
            {
                case DatabaseType.SQL_LITE:
                    builder.Property(x => x.Version).IsRowVersion().HasConversion(new SqliteTimestampConverter())
                        .HasColumnType("BLOB").HasDefaultValueSql("CURRENT_TIMESTAMP");

                    break;
                case DatabaseType.POSTGRESQL:
                    builder.Property(x => x.Version).IsRowVersion();
                    break;
            }
        }

        
    }
}
