using Weather.Model.ComplexSearchable;
using Weather.Model.Searchable;

namespace Weather.Model.Dto.Read
{
    public class SearchableSampleDto
    {
        public SearchableBme? SearchableBme { get; set; }
        public ComplexSearchableBme? ComplexSearchableBme { get; set; }
        public SearchableDmi? SearchableDmi { get; set; }
        public ComplexSearchableDmi? ComplexSearchableDmi { get; set; }
        public SearchableDs? SearchableDs { get; set; }
        public ComplexSearchableDs? ComplexSearchableDs { get; set; }
        public SearchableScd? SearchableScd { get; set; }
        public ComplexSearchableScd? ComplexSearchableScd { get; set; }
        public required string Comment { get; set; }
    }
}