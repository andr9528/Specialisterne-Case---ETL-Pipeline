using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Model.Entity;
using Weather.Abstraction.Interfaces.Model.Searchable;

namespace Weather.Model.Dto.Read
{
    public class ReadDtoScd : ISearchableScd, IReadDto, ISensor

    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Guid ReaderId { get; set; }

        /// <inheritdoc />
        public int CarbonDioxide { get; set; }

        /// <inheritdoc />
        public DateTime ObservedAt { get; set; }

        /// <inheritdoc />
        public DateTime PulledAt { get; set; }

        public required string HumidityUnit { get; set; }

        public required string TemperatureUnit { get; set; }

        public required string CarbonDioxideUnit { get; set; }

        public float Humidity { get; set; }

        public float Temperature { get; set; }
    }
}