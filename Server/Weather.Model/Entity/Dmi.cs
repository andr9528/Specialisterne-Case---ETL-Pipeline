using System.Text.Json.Serialization;
using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Model.Entity;

namespace Weather.Model.Entity
{
    public class Dmi : IDmi
    {
        private int id;

        /// <inheritdoc />
        public int Id
        {
            get => id;
            set => throw new InvalidOperationException(
                $"{nameof(Id)} cannot be changed after creation of {nameof(Bme)} entity");
        }

        /// <inheritdoc />
        public Guid DmiId { get; set; }

        /// <inheritdoc />
        public DmiParameter ParameterId { get; set; }

        /// <inheritdoc />
        public int StationId { get; set; }

        /// <inheritdoc />
        public uint Version { get; set; }

        /// <inheritdoc />
        public DateTime CreatedDateTime { get; set; }

        /// <inheritdoc />
        public DateTime UpdatedDateTime { get; set; }

        /// <inheritdoc />
        public DateTime ObservedAt { get; set; }

        /// <inheritdoc />
        public DateTime PulledAt { get; set; }

        /// <inheritdoc />
        public double Value { get; set; }

        /// <inheritdoc />
        public string ValueUnit => GetValueUnit();

        private string GetValueUnit()
        {
            return ParameterId switch
            {
                DmiParameter.TEMP_DRY => "°C",
                DmiParameter.HUMIDITY => "%",
                DmiParameter.PRESSURE => "hPa",
                _ => throw new ArgumentOutOfRangeException($"{nameof(ParameterId)} contained an unexpected value"),
            };
        }

        /// <summary>
        /// Constructor for Entity Framework Core to use.
        /// Enables the 'Id' to be immutable after the entity is created, which is a good practice for entities.
        /// Use of [JsonConstructor] is what makes Entity Framework Core use this constructor instead of the parameterless one, which is the default behavior.
        /// </summary>
        /// <param name="id"></param>
        [JsonConstructor]
        public Dmi(int id)
        {
            this.id = id;
        }

        public Dmi()
        {
        }
    }
}