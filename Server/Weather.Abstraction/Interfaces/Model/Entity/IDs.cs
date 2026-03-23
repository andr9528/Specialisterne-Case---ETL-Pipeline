using Weather.Abstraction.Interfaces.Model.Searchable;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Abstraction.Interfaces.Model.Entity
{
    public interface IDs : ISearchableDs, IEntity, ISensor
    {
        float Temperature { get; set; }
        string TemperatureUnit { get; }
    }
}