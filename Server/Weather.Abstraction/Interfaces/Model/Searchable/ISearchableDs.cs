using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Abstraction.Interfaces.Model.Searchable
{
    public interface ISearchableDs : ISearchable
    {
        Guid ReaderId { get; set; }
        Location Location { get; set; }
    }
}