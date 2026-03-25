using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Abstraction.Interfaces.Startup;

namespace Weather.Server.Controllers.Core
{
    public sealed record EntityControllerDependencies<TEntity, TSearchable, TDto>(
        IEntityQueryService<TEntity, TSearchable> QueryService,
        IReadDtoFactory<TEntity, TDto> ReadDtoFactory) where TEntity : class, IEntity
        where TSearchable : class, ISearchable, new()
        where TDto : class, IReadDto;
}