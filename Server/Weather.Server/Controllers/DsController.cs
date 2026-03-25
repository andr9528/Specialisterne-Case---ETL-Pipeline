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
    public class DsController : EntityController<Ds, SearchableDs, DsController, ComplexSearchableDs, ReadDtoDs>
    {
        /// <inheritdoc />
        public DsController(
            EntityControllerDependencies<Ds, SearchableDs, ReadDtoDs> dependencies, ILogger<DsController> logger) : base(dependencies, logger)
        {
        }
    }
}