using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Model.Searchable;

namespace Weather.Model.Searchable
{
    public class SearchableDs : ISearchableDs
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Guid ReaderId { get; set; }

        /// <inheritdoc />
        public Location Location { get; set; }
    }
}