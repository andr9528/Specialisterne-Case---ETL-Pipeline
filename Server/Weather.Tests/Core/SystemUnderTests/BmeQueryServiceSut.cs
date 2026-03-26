using Weather.Persistence;
using Weather.Persistence.Services;

namespace Weather.Tests.Core.SystemUnderTests
{
    internal sealed class BmeQueryServiceSut : IDisposable
    {
        public BmeQueryServiceSut(WeatherDatabaseContext context, WeatherEntityFactory factory, BmeQueryService service)
        {
            Context = context;
            Factory = factory;
            Service = service;
        }

        public WeatherDatabaseContext Context { get; }
        public WeatherEntityFactory Factory { get; }
        public BmeQueryService Service { get; }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}