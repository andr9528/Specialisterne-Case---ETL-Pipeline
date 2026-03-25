using Microsoft.AspNetCore.Mvc;
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
    public class BmeController : EntityController<Bme, SearchableBme, BmeController, ComplexSearchableBme, ReadDtoBme>
    {
        /// <inheritdoc />
        public BmeController(EntityControllerDependencies<Bme, SearchableBme, ReadDtoBme> dependencies, ILogger<BmeController> logger) : base(dependencies, logger)
        {
        }
    }
}