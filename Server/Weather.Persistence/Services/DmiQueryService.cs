using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.ComplexSearchable;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Persistence.Core;
using Weather.Persistence.Extensions;

namespace Weather.Persistence.Services
{
    public class DmiQueryService : BaseEntityQueryService<WeatherDatabaseContext, Dmi, SearchableDmi>
    {
        /// <inheritdoc />
        public DmiQueryService(WeatherDatabaseContext context) : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<Dmi> AddComplexQueryArguments(
            IQueryable<Dmi> query, IComplexSearchable<SearchableDmi> complex)
        {
            if (complex is not ComplexSearchableDmi complexSearchableDmi)
                throw new ArgumentException(
                    $"Expected {nameof(complex)} to be of type {nameof(ComplexSearchableDmi)}, but it wasn't.");

            query = query.ApplyLastXDaysFilter(complex.LastXDaysObservedAt, x => x.ObservedAt);
            query = query.ApplyLastXDaysFilter(complex.LastXDaysPulledAt, x => x.PulledAt);

            query = query.ApplyAfterDateTime(complex.ObservedAtAfterThisDateTime, x => x.ObservedAt);
            query = query.ApplyBeforeDateTime(complex.ObservedAtBeforeThisDateTime, x => x.ObservedAt);
            query = query.ApplyAfterDateTime(complex.PulledAtAfterThisDateTime, x => x.PulledAt);
            query = query.ApplyBeforeDateTime(complex.PulledAtBeforeThisDateTime, x => x.PulledAt);

            return query.ApplyOrderingQueryArguments(complexSearchableDmi);
        }

        /// <inheritdoc />
        protected override IEnumerable<Dmi> ApplyComplexNonDatabaseQueryArguments(
            IEnumerable<Dmi> entities, IComplexSearchable<SearchableDmi> complex)
        {
            return entities;
        }

        /// <inheritdoc />
        protected override IQueryable<Dmi> GetBaseQuery()
        {
            return context.Dmi.AsQueryable();
        }

        /// <inheritdoc />
        protected override IQueryable<Dmi> AddQueryArguments(SearchableDmi searchable, IQueryable<Dmi> query)
        {
            if (searchable.DmiId != Guid.Empty)
                query = query.Where(x => x.DmiId == searchable.DmiId);

            if (searchable.ParameterId != default)
                query = query.Where(x => x.ParameterId == searchable.ParameterId);

            if (searchable.StationId != 0)
                query = query.Where(x => x.StationId == searchable.StationId);

            return query;
        }
    }
}