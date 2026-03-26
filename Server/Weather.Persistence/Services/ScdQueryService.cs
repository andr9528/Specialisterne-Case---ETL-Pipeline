using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.ComplexSearchable;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Persistence.Core;
using Weather.Persistence.Extensions;

namespace Weather.Persistence.Services
{
    public class ScdQueryService : BaseEntityQueryService<WeatherDatabaseContext, Scd, SearchableScd>
    {
        /// <inheritdoc />
        public ScdQueryService(WeatherDatabaseContext context) : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<Scd> AddComplexQueryArguments(IQueryable<Scd> query, IComplexSearchable<SearchableScd> complex)
        {
            if (complex is not ComplexSearchableScd complexSearchableScd)
            {
                throw new ArgumentException(
                    $"Expected {nameof(complex)} to be of type {nameof(ComplexSearchableDs)}, but it wasn't.");
            }

            return query.ApplyOrderingQueryArguments(complexSearchableScd);
        }

        /// <inheritdoc />
        protected override IEnumerable<Scd> ApplyComplexNonDatabaseQueryArguments(IEnumerable<Scd> entities, IComplexSearchable<SearchableScd> complex)
        {
            return entities;
        }

        /// <inheritdoc />
        protected override IQueryable<Scd> GetBaseQuery()
        {
            return context.Scd.AsQueryable();
        }

        /// <inheritdoc />
        protected override IQueryable<Scd> AddQueryArguments(SearchableScd searchable, IQueryable<Scd> query)
        {
            if (searchable.ReaderId != Guid.Empty)
            {
                query = query.Where(x => x.ReaderId == searchable.ReaderId);
            }

            if (searchable.CarbonDioxide != 0)
            {
                query = query.Where(x => x.CarbonDioxide == searchable.CarbonDioxide);
            }

            return query;
        }
    }
}