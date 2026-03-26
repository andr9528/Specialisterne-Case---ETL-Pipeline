using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Abstraction.Interfaces.Startup;
using Weather.Model.ComplexSearchable;
using Weather.Model.Dto.Read;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Persistence;
using Weather.Persistence.Services;
using Weather.Server.Controllers.Core;
using Weather.Server.Factory.ReadDto;
using Weather.Server.Startup;
using Weather.Services;
using Weather.Startup;
using Weather.Startup.Modules;

namespace Weather.Server
{
    public class ApiStartup : ModularStartup<IApplicationBuilder>
    {
        private readonly IConfiguration configuration;
        private readonly ConfigurationService configurationService;

        public ApiStartup()
        {
            configurationService = new ConfigurationService();
            configuration = configurationService.BuildConfiguration();

            AddModule(new LoggingStartupModule(configurationService.GetApplicationDataPath()));
            AddModule(new SwaggerStartupModule("Weather"));

            AddModule(new EntityQueryServiceStartupModule<BmeQueryService, Bme, SearchableBme>());
            AddModule(new EntityQueryServiceStartupModule<DmiQueryService, Dmi, SearchableDmi>());
            AddModule(new EntityQueryServiceStartupModule<DsQueryService, Ds, SearchableDs>());
            AddModule(new EntityQueryServiceStartupModule<ScdQueryService, Scd, SearchableScd>());

            AddModule(new DatabaseContextStartupModule<WeatherDatabaseContext>(
                configurationService.ConfigureDatabaseOptions, StartupHandling.CREATE));
        }

        /// <inheritdoc />
        protected override void ConfigureApplication(IApplicationBuilder app)
        {
            base.ConfigureApplication(app);

            if (app is not WebApplication webApplication)
                throw new InvalidOperationException(
                    $"Expected Supplied App to be of type {nameof(WebApplication)}, but it was a {app.GetType().Name}.");

            webApplication.UseHttpsRedirection();
            webApplication.UseCors(x =>
                x.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials());
            webApplication.UseAuthorization();

            webApplication.MapControllers();
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            services.AddTransient<IComplexSearchable<SearchableBme>, ComplexSearchableBme>();
            services.AddTransient<IComplexSearchable<SearchableDmi>, ComplexSearchableDmi>();
            services.AddTransient<IComplexSearchable<SearchableDs>, ComplexSearchableDs>();
            services.AddTransient<IComplexSearchable<SearchableScd>, ComplexSearchableScd>();

            services.AddScoped<IReadDtoFactory<Bme, ReadDtoBme>, BmeReadDtoFactory>();
            services.AddScoped<IReadDtoFactory<Dmi, ReadDtoDmi>, DmiReadDtoFactory>();
            services.AddScoped<IReadDtoFactory<Ds, ReadDtoDs>, DsReadDtoFactory>();
            services.AddScoped<IReadDtoFactory<Scd, ReadDtoScd>, ScdReadDtoFactory>();

            services.AddScoped(typeof(EntityControllerDependencies<,,>));
            services.AddScoped(typeof(OverviewControllerDependencies));

            services.AddCors();
        }
    }
}