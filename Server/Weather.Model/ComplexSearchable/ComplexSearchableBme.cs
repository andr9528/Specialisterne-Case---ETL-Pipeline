using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Searchable;

namespace Weather.Model.ComplexSearchable
{
    public class ComplexSearchableBme : IComplexSearchable<SearchableBme>
    {
        /// <inheritdoc />
        public SearchableBme Searchable { get; set; } = new();

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

        public float? AboveHumidity { get; set; }
        public float? AbovePressure { get; set; }
        public float? AboveTemperature { get; set; }
        public float? BelowHumidity { get; set; }
        public float? BelowPressure { get; set; }
        public float? BelowTemperature { get; set; }
    }
}