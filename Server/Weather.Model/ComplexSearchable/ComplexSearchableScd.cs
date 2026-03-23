using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Searchable;

namespace Weather.Model.ComplexSearchable
{
    public class ComplexSearchableScd : IComplexSearchable<SearchableScd>
    {
        /// <inheritdoc />
        public SearchableScd Searchable { get; set; } = new SearchableScd();
    }
}