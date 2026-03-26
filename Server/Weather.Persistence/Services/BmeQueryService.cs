using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.ComplexSearchable;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Persistence.Core;
using Weather.Persistence.Extensions;

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
            if (complex is not ComplexSearchableBme complexSearchableBme)
            {
                throw new ArgumentException(
                    $"Expected {nameof(complex)} to be of type {nameof(ComplexSearchableBme)}, but it wasn't.");
            }

            query = query.ApplyLastXDaysFilter(complex.LastXDaysObservedAt, x => x.ObservedAt);
            query = query.ApplyLastXDaysFilter(complex.LastXDaysPulledAt, x => x.PulledAt);

            return query.ApplyOrderingQueryArguments(complexSearchableBme);
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
            if (searchable.Location != default)
            {
                query = query.Where(x => x.Location == searchable.Location);
            }

            if (searchable.ReaderId != Guid.Empty)
            {
                query = query.Where(x => x.ReaderId == searchable.ReaderId);
            }

            return query;
        }
    }
}