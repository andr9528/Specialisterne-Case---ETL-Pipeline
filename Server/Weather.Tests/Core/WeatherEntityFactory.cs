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

    public async Task<Dmi> AddDmi(
        int? id = null, Guid? dmiId = null, DmiParameter? parameterId = null, int stationId = 12345,
        DateTime? observedAt = null, DateTime? pulledAt = null, double value = 12.3d)
    {
        var entity = new Dmi(id ?? 0)
        {
            DmiId = dmiId ?? Guid.NewGuid(),
            ParameterId = parameterId ?? DmiParameter.TEMP_DRY,
            StationId = stationId,
            ObservedAt = observedAt ?? DateTime.UtcNow,
            PulledAt = pulledAt ?? DateTime.UtcNow,
            Value = value
        };

        await context.Dmi.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<Ds> AddDs(
        int? id = null, Guid? readerId = null, Location? location = null, DateTime? observedAt = null,
        DateTime? pulledAt = null, float temperature = 20.5f)
    {
        var entity = new Ds(id ?? 0)
        {
            ReaderId = readerId ?? Guid.NewGuid(),
            Location = location ?? Location.INSIDE,
            ObservedAt = observedAt ?? DateTime.UtcNow,
            PulledAt = pulledAt ?? DateTime.UtcNow,
            Temperature = temperature
        };

        await context.Ds.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<Scd> AddScd(
        int? id = null, Guid? readerId = null, int carbonDioxide = 500, DateTime? observedAt = null,
        DateTime? pulledAt = null, float humidity = 45.0f, float temperature = 20.5f)
    {
        var entity = new Scd(id ?? 0)
        {
            ReaderId = readerId ?? Guid.NewGuid(),
            CarbonDioxide = carbonDioxide,
            ObservedAt = observedAt ?? DateTime.UtcNow,
            PulledAt = pulledAt ?? DateTime.UtcNow,
            Humidity = humidity,
            Temperature = temperature
        };

        await context.Scd.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }
}