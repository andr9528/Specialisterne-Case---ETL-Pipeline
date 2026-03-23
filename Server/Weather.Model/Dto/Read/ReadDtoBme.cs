using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Model.Entity;
using Weather.Abstraction.Interfaces.Model.Searchable;

namespace Weather.Model.Dto.Read
{
    public class ReadDtoBme : ISearchableBme, IReadDto, ISensor
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Location Location { get; set; }

        /// <inheritdoc />
        public Guid ReaderId { get; set; }

        public required string HumidityUnit { get; set; }
        public required string PressureUnit { get; set; }
        public required string TemperatureUnit { get; set; }

        public float Humidity { get; set; }
        public float Pressure { get; set; }
        public float Temperature { get; set; }

        /// <inheritdoc />
        public DateTime ObservedAt { get; set; }

        /// <inheritdoc />
        public DateTime PulledAt { get; set; }
    }
}