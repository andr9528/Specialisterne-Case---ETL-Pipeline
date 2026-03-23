using Weather.Abstraction.Interfaces.Model.Searchable;

namespace Weather.Model.Searchable
{
    public class SearchableScd : ISearchableScd
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Guid ReaderId { get; set; }

        /// <inheritdoc />
        public int CarbonDioxide { get; set; }
    }
}