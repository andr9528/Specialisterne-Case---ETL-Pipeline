using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Searchable;

namespace Weather.Model.ComplexSearchable
{
    public class ComplexSearchableBme : IComplexSearchable<SearchableBme>
    {
        /// <inheritdoc />
        public SearchableBme Searchable { get; set; } = new SearchableBme();
    }
}