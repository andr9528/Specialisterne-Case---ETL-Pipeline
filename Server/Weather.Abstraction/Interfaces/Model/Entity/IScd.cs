using Weather.Abstraction.Interfaces.Model.Searchable;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Abstraction.Interfaces.Model.Entity
{
    public interface IScd : ISearchableScd, IEntity, ISensor
    {
        string HumidityUnit { get; }
        string TemperatureUnit { get; }
        string CarbonDioxideUnit { get; }

        float Humidity { get; set; }
        float Temperature { get; set; }
    }
}