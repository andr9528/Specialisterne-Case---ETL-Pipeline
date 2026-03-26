using Weather.Persistence;
using Weather.Persistence.Services;

namespace Weather.Tests.Core.SystemUnderTests
{
    internal sealed class ScdQueryServiceSut : IDisposable
    {
        public ScdQueryServiceSut(WeatherDatabaseContext context, WeatherEntityFactory factory, ScdQueryService service)
        {
            Context = context;
            Factory = factory;
            Service = service;
        }

        public WeatherDatabaseContext Context { get; }
        public WeatherEntityFactory Factory { get; }
        public ScdQueryService Service { get; }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}