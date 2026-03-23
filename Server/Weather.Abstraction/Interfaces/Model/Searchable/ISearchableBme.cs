using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Abstraction.Interfaces.Model.Searchable;

public interface ISearchableBme : ISearchable
{
    Location Location { get; set; }
    Guid ReaderId { get; set; }

}
