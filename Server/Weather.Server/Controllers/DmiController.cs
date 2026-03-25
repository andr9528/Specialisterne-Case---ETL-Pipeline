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
    public class DmiController : EntityController<Dmi, SearchableDmi, DmiController, ComplexSearchableDmi, ReadDtoDmi>
    {
        /// <inheritdoc />
        public DmiController(
            EntityControllerDependencies<Dmi, SearchableDmi, ReadDtoDmi> dependencies, ILogger<DmiController> logger) : base(dependencies, logger)
        {
        }
    }
}