using Weather.Persistence;
using Weather.Persistence.Services;

namespace Weather.Tests.Core.SystemUnderTests
{
    internal sealed class DsQueryServiceSut : IDisposable
    {
        public DsQueryServiceSut(WeatherDatabaseContext context, WeatherEntityFactory factory, DsQueryService service)
        {
            Context = context;
            Factory = factory;
            Service = service;
        }

        public WeatherDatabaseContext Context { get; }
        public WeatherEntityFactory Factory { get; }
        public DsQueryService Service { get; }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}