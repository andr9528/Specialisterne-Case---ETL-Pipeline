using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Searchable;

namespace Weather.Model.ComplexSearchable
{
    public class ComplexSearchableDs : IComplexSearchable<SearchableDs>
    {
        /// <inheritdoc />
        public SearchableDs Searchable { get; set; } = new SearchableDs();

        /// <inheritdoc />
        public OrderDirection? OrderByObservedAt { get; set; }

        /// <inheritdoc />
        public OrderDirection? OrderByPulledAt { get; set; }

        /// <inheritdoc />
        public int? LastXDaysObservedAt { get; set; }

        /// <inheritdoc />
        public int? LastXDaysPulledAt { get; set; }
    }
}