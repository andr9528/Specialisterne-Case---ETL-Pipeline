using Weather.Abstraction.Interfaces.Model.Searchable;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Abstraction.Interfaces.Model.Entity;

public interface IBme : ISearchableBme, IEntity, ISensor
{
    string HumidityUnit { get; }
    string PressureUnit { get; }
    string TemperatureUnit { get; }

    float Humidity { get; set; }
    float Pressure { get; set; }
    float Temperature { get; set; }
}
