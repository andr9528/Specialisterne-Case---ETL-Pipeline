using Weather.Persistence.Services;
using Weather.Tests.Core.SystemUnderTests;

namespace Weather.Tests.Core
{
    public static class BaseDatabaseTestExtensions
    {
        internal static BmeQueryServiceSut CreateBmeQueryServiceSut<TTest>(this TTest test) where TTest : BaseDatabaseTest
        {
            var context = test.CreateContext();
            var factory = new WeatherEntityFactory(context);
            var service = new BmeQueryService(context);

            return new BmeQueryServiceSut(context, factory, service);
        }
    }
}