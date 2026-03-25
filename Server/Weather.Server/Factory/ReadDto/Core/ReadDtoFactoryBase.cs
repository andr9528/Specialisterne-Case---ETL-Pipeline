using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Abstraction.Interfaces.Startup;
using Weather.Model.Extensions;

namespace Weather.Server.Factory.ReadDto.Core
{
    public abstract class ReadDtoFactoryBase<TEntity, TDto> : IReadDtoFactory<TEntity, TDto>
        where TEntity : class, IEntity where TDto : class, IReadDto
    {
        public virtual TDto Create(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            TDto dto = entity.PackToReadDto<TDto>();
            MapAdditionalProperties(entity, dto);

            return dto;
        }

        protected virtual void MapAdditionalProperties(TEntity entity, TDto dto)
        {
        }
    }
}