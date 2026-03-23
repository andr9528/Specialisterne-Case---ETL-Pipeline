using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Persistence.Core;

namespace Weather.Persistence.Services
{
    public class BmeQueryService : BaseEntityQueryService<WeatherDatabaseContext, Bme, SearchableBme>
    {
        /// <inheritdoc />
        public BmeQueryService(WeatherDatabaseContext context) : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<Bme> AddComplexQueryArguments(IQueryable<Bme> query, IComplexSearchable<SearchableBme> complex)
        {
            return query;
        }

        /// <inheritdoc />
        protected override IEnumerable<Bme> ApplyComplexNonDatabaseQueryArguments(IEnumerable<Bme> entities, IComplexSearchable<SearchableBme> complex)
        {
            return entities;
        }

        /// <inheritdoc />
        protected override IQueryable<Bme> GetBaseQuery()
        {
            return context.Bme.AsQueryable();
        }

        /// <inheritdoc />
        protected override IQueryable<Bme> AddQueryArguments(SearchableBme searchable, IQueryable<Bme> query)
        {
            return query;
        }
    }
}