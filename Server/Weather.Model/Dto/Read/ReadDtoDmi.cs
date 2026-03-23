using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Model.Entity;
using Weather.Abstraction.Interfaces.Model.Searchable;

namespace Weather.Model.Dto.Read
{
    public class ReadDtoDmi : ISearchableDmi, IReadDto, ISensor
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Guid DmiId { get; set; }

        /// <inheritdoc />
        public DmiParameter ParameterId { get; set; }

        /// <inheritdoc />
        public int StationId { get; set; }

        /// <inheritdoc />
        public DateTime ObservedAt { get; set; }

        /// <inheritdoc />
        public DateTime PulledAt { get; set; }

        public double Value { get; set; }

        public required string ValueUnit { get; set; }
    }
}