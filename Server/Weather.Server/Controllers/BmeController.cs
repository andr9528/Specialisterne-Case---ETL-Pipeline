using Microsoft.AspNetCore.Mvc;
using Weather.Abstraction.Interfaces.Persistence;
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
        public BmeController(IEntityQueryService<Bme, SearchableBme> entityService, ILogger<BmeController> logger) : base(entityService, logger)
        {
        }

        /// <inheritdoc />
        protected override ReadDtoBme BuildDataTransferObject(Bme entity)
        {
            throw new NotImplementedException();
        }
    }
}