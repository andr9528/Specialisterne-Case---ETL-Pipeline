using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Model.Searchable;

namespace Weather.Model.Searchable
{
    public class SearchableDmi : ISearchableDmi
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Guid DmiId { get; set; }

        /// <inheritdoc />
        public DmiParameter ParameterId { get; set; }

        /// <inheritdoc />
        public int StationId { get; set; }
    }
}