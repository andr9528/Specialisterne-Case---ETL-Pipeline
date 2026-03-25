using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Persistence.Core;

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
            return query;
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