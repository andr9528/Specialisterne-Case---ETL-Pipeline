using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Searchable;

namespace Weather.Model.ComplexSearchable
{
    public class ComplexSearchableDmi : IComplexSearchable<SearchableDmi>
    {
        /// <inheritdoc />
        public SearchableDmi Searchable { get; set; } = new();

        /// <inheritdoc />
        public OrderDirection? OrderByObservedAt { get; set; }

        /// <inheritdoc />
        public OrderDirection? OrderByPulledAt { get; set; }

        /// <inheritdoc />
        public int? LastXDaysObservedAt { get; set; }

        /// <inheritdoc />
        public int? LastXDaysPulledAt { get; set; }

        /// <inheritdoc />
        public DateTime? ObservedAtAfterThisDateTime { get; set; }

        /// <inheritdoc />
        public DateTime? ObservedAtBeforeThisDateTime { get; set; }

        /// <inheritdoc />
        public DateTime? PulledAtAfterThisDateTime { get; set; }

        /// <inheritdoc />
        public DateTime? PulledAtBeforeThisDateTime { get; set; }
    }
}