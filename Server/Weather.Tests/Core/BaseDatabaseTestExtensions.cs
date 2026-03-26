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

        internal static DmiQueryServiceSut CreateDmiQueryServiceSut<TTest>(this TTest test) where TTest : BaseDatabaseTest
        {
            var context = test.CreateContext();
            var factory = new WeatherEntityFactory(context);
            var service = new DmiQueryService(context);

            return new DmiQueryServiceSut(context, factory, service);
        }

        internal static DsQueryServiceSut CreateDsQueryServiceSut<TTest>(this TTest test) where TTest : BaseDatabaseTest
        {
            var context = test.CreateContext();
            var factory = new WeatherEntityFactory(context);
            var service = new DsQueryService(context);

            return new DsQueryServiceSut(context, factory, service);
        }

        internal static ScdQueryServiceSut CreateScdQueryServiceSut<TTest>(this TTest test) where TTest : BaseDatabaseTest
        {
            var context = test.CreateContext();
            var factory = new WeatherEntityFactory(context);
            var service = new ScdQueryService(context);

            return new ScdQueryServiceSut(context, factory, service);
        }
    }
}