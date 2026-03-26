using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Abstraction.Interfaces.Startup;
using Weather.Model.ComplexSearchable;
using Weather.Model.Dto.Read;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Server.Controllers.Core;

namespace Weather.Server.Controllers
{
    [Route(Constants.ROUTE_TEMPLATE)]
    [ApiController]
    public class OverviewController : ControllerBase
    {
        private readonly IEntityQueryService<Bme, SearchableBme> bmeQueryService;
        private readonly IEntityQueryService<Dmi, SearchableDmi> dmiQueryService;
        private readonly IEntityQueryService<Ds, SearchableDs> dsQueryService;
        private readonly IEntityQueryService<Scd, SearchableScd> scdQueryService;

        private readonly IReadDtoFactory<Bme, ReadDtoBme> bmeReadDtoFactory;
        private readonly IReadDtoFactory<Dmi, ReadDtoDmi> dmiReadDtoFactory;
        private readonly IReadDtoFactory<Ds, ReadDtoDs> dsReadDtoFactory;
        private readonly IReadDtoFactory<Scd, ReadDtoScd> scdReadDtoFactory;

        private readonly ILogger<OverviewController> logger;

        public OverviewController(OverviewControllerDependencies dependencies, ILogger<OverviewController> logger)
        {
            bmeQueryService = dependencies.BmeQueryService;
            dmiQueryService = dependencies.DmiQueryService;
            dsQueryService = dependencies.DsQueryService;
            scdQueryService = dependencies.ScdQueryService;

            bmeReadDtoFactory = dependencies.BmeReadDtoFactory;
            dmiReadDtoFactory = dependencies.DmiReadDtoFactory;
            dsReadDtoFactory = dependencies.DsReadDtoFactory;
            scdReadDtoFactory = dependencies.ScdReadDtoFactory;

            this.logger = logger;
        }

        /// <summary>
        /// Retrieves the latest readings from all available sensors.
        /// </summary>
        /// <returns>
        /// An overview containing the most recent reading for each sensor type.
        /// Returns 204 No Content if a reading could not be found for one or more sensors.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<OverviewLatestReadingsDto>> GetLatestSensorReadings()
        {
            try
            {
                Bme? latestBme = await bmeQueryService.GetEntityComplex(new ComplexSearchableBme
                {
                    OrderByObservedAt = OrderDirection.DESCENDING,
                });

                Dmi? latestDmi = await dmiQueryService.GetEntityComplex(new ComplexSearchableDmi
                {
                    OrderByObservedAt = OrderDirection.DESCENDING,
                });

                Ds? latestDs = await dsQueryService.GetEntityComplex(new ComplexSearchableDs
                {
                    OrderByObservedAt = OrderDirection.DESCENDING,
                });

                Scd? latestScd = await scdQueryService.GetEntityComplex(new ComplexSearchableScd
                {
                    OrderByObservedAt = OrderDirection.DESCENDING,
                });

                if (ShouldReturnNoContent(latestBme, latestDmi, latestDs, latestScd))
                    return NoContent();

                var result = new OverviewLatestReadingsDto
                {
                    Bme = bmeReadDtoFactory.Create(latestBme!),
                    Dmi = dmiReadDtoFactory.Create(latestDmi!),
                    Ds = dsReadDtoFactory.Create(latestDs!),
                    Scd = scdReadDtoFactory.Create(latestScd!),
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An exception was caught while attempting to get latest sensor readings");
                throw;
            }
        }

        private bool ShouldReturnNoContent(Bme? bme, Dmi? dmi, Ds? ds, Scd? scd)
        {
            var missing = new List<string>();

            if (bme is null) missing.Add(nameof(Bme));
            if (dmi is null) missing.Add(nameof(Dmi));
            if (ds is null) missing.Add(nameof(Ds));
            if (scd is null) missing.Add(nameof(Scd));

            if (missing.Count == 0)
                return false;

            logger.LogWarning("Missing latest sensor readings for: {MissingSensors}", string.Join(", ", missing));

            return true;
        }
    }
}