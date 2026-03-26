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
    public class ScdController : EntityController<Scd, SearchableScd, ScdController, ComplexSearchableScd, ReadDtoScd>
    {
        /// <inheritdoc />
        public ScdController(
            EntityControllerDependencies<Scd, SearchableScd, ReadDtoScd> dependencies,
            ILogger<ScdController> logger) : base(dependencies, logger)
        {
        }
    }
}