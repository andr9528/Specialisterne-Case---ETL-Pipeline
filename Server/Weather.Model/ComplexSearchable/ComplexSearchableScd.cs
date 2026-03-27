using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Searchable;

namespace Weather.Model.ComplexSearchable
{
    public class ComplexSearchableScd : IComplexSearchable<SearchableScd>
    {
        /// <inheritdoc />
        public SearchableScd Searchable { get; set; } = new();

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

        public int? AboveCarbonDioxide { get; set; }
        public float? AboveHumidity { get; set; }
        public float? AboveTemperature { get; set; }
        public int? BelowCarbonDioxide { get; set; }
        public float? BelowHumidity { get; set; }
        public float? BelowTemperature { get; set; }
    }
}