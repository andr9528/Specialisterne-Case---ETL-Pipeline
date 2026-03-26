using Weather.Persistence;
using Weather.Persistence.Services;

namespace Weather.Tests.Core.SystemUnderTests
{
    internal sealed class DmiQueryServiceSut : IDisposable
    {
        public DmiQueryServiceSut(WeatherDatabaseContext context, WeatherEntityFactory factory, DmiQueryService service)
        {
            Context = context;
            Factory = factory;
            Service = service;
        }

        public WeatherDatabaseContext Context { get; }
        public WeatherEntityFactory Factory { get; }
        public DmiQueryService Service { get; }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}