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
        protected override IQueryable<Bme> AddComplexQueryArguments(
            IQueryable<Bme> query, IComplexSearchable<SearchableBme> complex)
        {
            if (complex is not ComplexSearchableBme complexSearchableBme)
                throw new ArgumentException(
                    $"Expected {nameof(complex)} to be of type {nameof(ComplexSearchableBme)}, but it wasn't.");

            query = query.ApplyLastXDaysFilter(complex.LastXDaysObservedAt, x => x.ObservedAt);
            query = query.ApplyLastXDaysFilter(complex.LastXDaysPulledAt, x => x.PulledAt);

            query = query.ApplyAfterDateTime(complex.ObservedAtAfterThisDateTime, x => x.ObservedAt);
            query = query.ApplyBeforeDateTime(complex.ObservedAtBeforeThisDateTime, x => x.ObservedAt);
            query = query.ApplyAfterDateTime(complex.PulledAtAfterThisDateTime, x => x.PulledAt);
            query = query.ApplyBeforeDateTime(complex.PulledAtBeforeThisDateTime, x => x.PulledAt);

            query = query.ApplyAboveValue(complexSearchableBme.AboveHumidity, x => x.Humidity);
            query = query.ApplyAboveValue(complexSearchableBme.AbovePressure, x => x.Pressure);
            query = query.ApplyAboveValue(complexSearchableBme.AboveTemperature, x => x.Temperature);

            query = query.ApplyBelowValue(complexSearchableBme.BelowHumidity, x => x.Humidity);
            query = query.ApplyBelowValue(complexSearchableBme.BelowPressure, x => x.Pressure);
            query = query.ApplyBelowValue(complexSearchableBme.BelowTemperature, x => x.Temperature);

            return query.ApplyOrderingQueryArguments(complexSearchableBme);
        }

        /// <inheritdoc />
        protected override IEnumerable<Bme> ApplyComplexNonDatabaseQueryArguments(
            IEnumerable<Bme> entities, IComplexSearchable<SearchableBme> complex)
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
                query = query.Where(x => x.Location == searchable.Location);

            if (searchable.ReaderId != Guid.Empty)
                query = query.Where(x => x.ReaderId == searchable.ReaderId);

            return query;
        }
    }
}