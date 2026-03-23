using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Server.Controllers.Core;

// Todo: Add Authorization / Authentication

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
    protected readonly IEntityQueryService<TEntity, TSearchable> entityService;
    protected readonly ILogger<TController> logger;

    protected EntityController(IEntityQueryService<TEntity, TSearchable> entityService, ILogger<TController> logger)
    {
        this.entityService = entityService;
        this.logger = logger;
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
            logger.LogError(e, "An exception was caught while attempting to get all entities of the controllers type.");
            throw;
        }
    }

    [HttpGet("id")]
    public virtual async Task<IActionResult> GetById(int id)
    {
        try
        {
            TEntity? entity = await entityService.GetEntity(new TSearchable { Id = id });

            if (entity is null)
            {
                logger.LogInformation("No entity found for id: {@Searchable}", id);

                return NoContent();
            }

            return Ok(BuildDataTransferObject(entity));
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
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
            logger.LogError(
                e,
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
            logger.LogError(
                e,
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
            logger.LogError(
                e,
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
            logger.LogError(
                e,
                "An exception was caught while attempting to get entities matching specified query of the controllers type. Query: {@Searchable}",
                complex);
            throw;
        }
    }

    private IEnumerable<TReadDto> BuildDataTransferObjects(IEnumerable<TEntity> entities) => entities.Select(BuildDataTransferObject);
    protected abstract TReadDto BuildDataTransferObject(TEntity entity);
}
