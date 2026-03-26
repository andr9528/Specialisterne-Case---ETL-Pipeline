using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Searchable;

namespace Weather.Model.ComplexSearchable
{
    public class ComplexSearchableScd : IComplexSearchable<SearchableScd>
    {
        /// <inheritdoc />
        public SearchableScd Searchable { get; set; } = new SearchableScd();

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