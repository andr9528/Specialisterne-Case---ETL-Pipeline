using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Model.Entity;
using Weather.Abstraction.Interfaces.Model.Searchable;

namespace Weather.Model.Dto.Read
{
    public class ReadDtoDs : ISearchableDs, IReadDto, ISensor
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Guid ReaderId { get; set; }

        /// <inheritdoc />
        public Location Location { get; set; }

        /// <inheritdoc />
        public DateTime ObservedAt { get; set; }

        /// <inheritdoc />
        public DateTime PulledAt { get; set; }

        public float Temperature { get; set; }

        public required string TemperatureUnit { get; set; }

        /// <inheritdoc />
        public required string LocalObservedAtHumanReadable { get; set; }

        /// <inheritdoc />
        public required string LocalPulledAtHumanReadable { get; set; }
    }
}