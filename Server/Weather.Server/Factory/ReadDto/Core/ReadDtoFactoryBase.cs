using System;
using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Model.Entity;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Abstraction.Interfaces.Startup;
using Weather.Model.Extensions;

namespace Weather.Server.Factory.ReadDto.Core
{
    public abstract class ReadDtoFactoryBase<TEntity, TDto, TFactory> : IReadDtoFactory<TEntity, TDto>
        where TEntity : class, IEntity 
        where TDto : class, IReadDto
        where TFactory : class, IReadDtoFactory<TEntity, TDto>
    {
        private readonly ILogger<TFactory> logger;

        protected ReadDtoFactoryBase(ILogger<TFactory> logger)
        {
            this.logger = logger;
        }

        public virtual TDto Create(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            TDto dto = entity.PackToReadDto<TDto>();
            MapCommonProperties(entity, dto);
            MapAdditionalProperties(entity, dto);

            dto.EnsureRequiredPropertiesAreSet();

            return dto;
        }

        private void MapCommonProperties(TEntity entity, TDto dto)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(dto);

            if (entity is not ISensor sensor)
            {
                return;
            }

            dto.LocalObservedAtHumanReadable = FormatAsHumanReadable(sensor.ObservedAt);
            dto.LocalPulledAtHumanReadable = FormatAsHumanReadable(sensor.PulledAt);
        }

        protected virtual string FormatAsHumanReadable(DateTime value)
        {
            if (value.Kind == DateTimeKind.Unspecified)
            {
                logger.LogWarning("DateTime with Unspecified kind encountered: {Value}", value);
            }

            DateTime localTime = value.Kind switch
            {
                DateTimeKind.Utc => value.ToLocalTime(),
                DateTimeKind.Local => value,
                DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc).ToLocalTime(),
                _ => throw new ArgumentOutOfRangeException()
            };

            return localTime.ToString("dd-MM-yyyy HH:mm");
        }

        protected virtual void MapAdditionalProperties(TEntity entity, TDto dto)
        {
        }
    }
}