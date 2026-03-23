using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Searchable;

namespace Weather.Model.ComplexSearchable
{
    public class ComplexSearchableDs : IComplexSearchable<SearchableDs>
    {
        /// <inheritdoc />
        public SearchableDs Searchable { get; set; } = new SearchableDs();
    }
}