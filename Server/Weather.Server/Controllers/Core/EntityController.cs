using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Numerics;
using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Abstraction.Interfaces.Startup;

namespace Weather.Server.Controllers.Core
{
    // Add the two lines below to any controller.
//[Route(Constants.ROUTE_TEMPLATE)]
//[ApiController]
    public abstract class EntityController<TEntity, TSearchable, TController, TComplex, TReadDto> : ControllerBase
        where TEntity : class, IEntity
        where TSearchable : class, ISearchable, new()
        where TController : ControllerBase
        where TComplex : IComplexSearchable<TSearchable>
        where TReadDto : class, IReadDto
    {
        private readonly IEntityQueryService<TEntity, TSearchable> entityService;
        private readonly ILogger<TController> logger;
        private readonly IReadDtoFactory<TEntity, TReadDto> readDtoFactory;

        protected EntityController(
            EntityControllerDependencies<TEntity, TSearchable, TReadDto> dependencies, ILogger<TController> logger)
        {
            this.logger = logger;
            entityService = dependencies.QueryService;
            readDtoFactory = dependencies.ReadDtoFactory;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            try
            {
                var entities = await entityService.GetAllEntities();
                var enumerable = entities.ToList();
                if (!enumerable.Any())
                {
                    logger.LogInformation("No entity found");

                    return NoContent();
                }

                var transferObjects = BuildDataTransferObjects(enumerable);
                return Ok(transferObjects);
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "An exception was caught while attempting to get all entities of the controllers type.");
                throw;
            }
        }

        [HttpGet("id")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            try
            {
                TEntity? entity = await entityService.GetEntity(new TSearchable {Id = id,});

                if (entity is null)
                {
                    logger.LogInformation("No entity found for id: {@Searchable}", id);

                    return NoContent();
                }

                return Ok(BuildDataTransferObject(entity));
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "An exception was caught while attempting to get an entity by id of the controllers type. Id: {Id}",
                    id);
                throw;
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetByQuery([FromBody] TSearchable searchable)
        {
            try
            {
                TEntity? entity = await entityService.GetEntity(searchable);

                if (entity is null)
                {
                    logger.LogInformation("No entity found for query: {@Searchable}", searchable);

                    return NoContent();
                }

                return Ok(BuildDataTransferObject(entity));
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "An exception was caught while attempting to get entity matching specified query of the controllers type. Query: {@Searchable}",
                    searchable);
                throw;
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetByComplexQuery([FromBody] TComplex complex)
        {
            try
            {
                TEntity? entity = await entityService.GetEntityComplex(complex);

                if (entity is null)
                {
                    logger.LogInformation("No entity found for query: {@Searchable}", complex);

                    return NoContent();
                }

                return Ok(BuildDataTransferObject(entity));
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "An exception was caught while attempting to get entity matching specified query of the controllers type. Query: {@Searchable}",
                    complex);
                throw;
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetAllByQuery([FromBody] TSearchable searchable)
        {
            try
            {
                var entities = await entityService.GetEntities(searchable);
                var enumerable = entities.ToList();
                if (!enumerable.Any())
                {
                    logger.LogInformation("No entity found for query: {@Searchable}", searchable);

                    return NoContent();
                }

                var transferObjects = BuildDataTransferObjects(enumerable);
                return Ok(transferObjects);
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "An exception was caught while attempting to get entities matching specified query of the controllers type. Query: {@Searchable}",
                    searchable);
                throw;
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetAllByComplexQuery([FromBody] TComplex complex)
        {
            try
            {
                var entities = await entityService.GetEntitiesComplex(complex);
                var enumerable = entities.ToList();
                if (!enumerable.Any())
                {
                    logger.LogInformation("No entity found for query: {@Searchable}", complex);

                    return NoContent();
                }

                var transferObjects = BuildDataTransferObjects(enumerable);
                return Ok(transferObjects);
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "An exception was caught while attempting to get entities matching specified query of the controllers type. Query: {@Searchable}",
                    complex);
                throw;
            }
        }

        private IEnumerable<TReadDto> BuildDataTransferObjects(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            var entityArray = entities as TEntity[] ?? entities.ToArray();

            var stopwatch = Stopwatch.StartNew();

            var dtos = entityArray.Select(BuildDataTransferObject).ToArray();

            stopwatch.Stop();

            double totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
            double millisecondsPerEntity = entityArray.Length == 0 ? 0 : totalMilliseconds / entityArray.Length;

            logger.LogInformation(
                "Mapped {EntityCount} entities of type {EntityType} to {DtoType} in {TotalMilliseconds:F2} ms ({MillisecondsPerEntity:F4} ms/entity).",
                entityArray.Length, typeof(TEntity).Name, typeof(TReadDto).Name, totalMilliseconds,
                millisecondsPerEntity);

            return dtos;
        }

        private TReadDto BuildDataTransferObject(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return readDtoFactory.Create(entity);
        }
    }
}