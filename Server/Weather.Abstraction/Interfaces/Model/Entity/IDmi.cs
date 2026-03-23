using Weather.Abstraction.Interfaces.Model.Searchable;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Abstraction.Interfaces.Model.Entity
{
    public interface IDmi : ISearchableDmi, IEntity, ISensor
    {
        double Value { get; set; }
        string ValueUnit { get; }
    }
}