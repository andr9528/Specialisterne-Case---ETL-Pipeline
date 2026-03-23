using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Model.Searchable;

namespace Weather.Model.Searchable
{
    public class SearchableBme : ISearchableBme
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Location Location { get; set; }

        /// <inheritdoc />
        public Guid ReaderId { get; set; }
    }
}