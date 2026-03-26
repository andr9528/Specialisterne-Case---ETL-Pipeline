using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.ComplexSearchable;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Persistence.Core;
using Weather.Persistence.Extensions;

namespace Weather.Persistence.Services
{
    public class DsQueryService : BaseEntityQueryService<WeatherDatabaseContext, Ds, SearchableDs>
    {
        /// <inheritdoc />
        public DsQueryService(WeatherDatabaseContext context) : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<Ds> AddComplexQueryArguments(IQueryable<Ds> query, IComplexSearchable<SearchableDs> complex)
        {
            if (complex is not ComplexSearchableDs complexSearchableDs)
            {
                throw new ArgumentException(
                    $"Expected {nameof(complex)} to be of type {nameof(ComplexSearchableDs)}, but it wasn't.");
            }

            query = query.ApplyLastXDaysFilter(complex.LastXDaysObservedAt, x => x.ObservedAt);
            query = query.ApplyLastXDaysFilter(complex.LastXDaysPulledAt, x => x.PulledAt);

            query = query.ApplyAfterDateTime(complex.ObservedAtAfterThisDateTime, x => x.ObservedAt);
            query = query.ApplyBeforeDateTime(complex.ObservedAtBeforeThisDateTime, x => x.ObservedAt);
            query = query.ApplyAfterDateTime(complex.PulledAtAfterThisDateTime, x => x.PulledAt);
            query = query.ApplyBeforeDateTime(complex.PulledAtBeforeThisDateTime, x => x.PulledAt);

            return query.ApplyOrderingQueryArguments(complexSearchableDs);
        }

        /// <inheritdoc />
        protected override IEnumerable<Ds> ApplyComplexNonDatabaseQueryArguments(IEnumerable<Ds> entities, IComplexSearchable<SearchableDs> complex)
        {
            return entities;
        }

        /// <inheritdoc />
        protected override IQueryable<Ds> GetBaseQuery()
        {
            return context.Ds.AsQueryable();
        }

        /// <inheritdoc />
        protected override IQueryable<Ds> AddQueryArguments(SearchableDs searchable, IQueryable<Ds> query)
        {
            if (searchable.ReaderId != Guid.Empty)
            {
                query = query.Where(x => x.ReaderId == searchable.ReaderId);
            }

            if (searchable.Location != default)
            {
                query = query.Where(x => x.Location == searchable.Location);
            }

            return query;
        }
    }
}