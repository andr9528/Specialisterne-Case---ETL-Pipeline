using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.Searchable;

namespace Weather.Model.ComplexSearchable
{
    public class ComplexSearchableDmi : IComplexSearchable<SearchableDmi>
    {
        /// <inheritdoc />
        public SearchableDmi Searchable { get; set; } = new SearchableDmi();
    }
}