using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Abstraction.Interfaces.Startup
{
    public interface IReadDtoFactory<TEntity, TDto> where TEntity : class, IEntity where TDto : class, IReadDto
    {
        TDto Create(TEntity entity);
    }
}