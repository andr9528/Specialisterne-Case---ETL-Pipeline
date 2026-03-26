using Weather.Abstraction.Enum;
using Weather.Model.Entity;
using Weather.Persistence;

namespace Weather.Tests.Core;

internal sealed class WeatherEntityFactory
{
    private readonly WeatherDatabaseContext context;

    public WeatherEntityFactory(WeatherDatabaseContext context)
    {
        this.context = context;
    }

    public async Task<Bme> AddBme(
        int? id = null, Guid? readerId = null, Location? location = null, DateTime? observedAt = null,
        DateTime? pulledAt = null, float temperature = 20.5f, float humidity = 45.0f, float pressure = 1013.2f)
    {
        var entity = new Bme(id ?? 0)
        {
            ReaderId = readerId ?? Guid.NewGuid(),
            Location = location ?? Location.INSIDE,
            ObservedAt = observedAt ?? DateTime.UtcNow,
            PulledAt = pulledAt ?? DateTime.UtcNow,
            Temperature = temperature,
            Humidity = humidity,
            Pressure = pressure
        };

        await context.Bme.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }
}