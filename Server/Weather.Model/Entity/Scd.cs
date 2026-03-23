using System.Text.Json.Serialization;
using Weather.Abstraction.Interfaces.Model.Entity;

namespace Weather.Model.Entity
{
    public class Scd : IScd
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
        public Guid ReaderId { get; set; }

        /// <inheritdoc />
        public int CarbonDioxide { get; set; }

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
        public string HumidityUnit => "%";

        /// <inheritdoc />
        public string TemperatureUnit => "°C";

        /// <inheritdoc />
        public string CarbonDioxideUnit => "ppm";

        /// <inheritdoc />
        public float Humidity { get; set; }

        /// <inheritdoc />
        public float Temperature { get; set; }

        /// <summary>
        /// Constructor for Entity Framework Core to use.
        /// Enables the 'Id' to be immutable after the entity is created, which is a good practice for entities.
        /// Use of [JsonConstructor] is what makes Entity Framework Core use this constructor instead of the parameterless one, which is the default behavior.
        /// </summary>
        /// <param name="id"></param>
        [JsonConstructor]
        private Scd(int id)
        {
            this.id = id;
        }
    }
}