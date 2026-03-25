using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using Weather.Abstraction.Interfaces.Dto;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Model.Extensions
{
    public static class EntityMappingExtension
    {
        private static readonly ConcurrentDictionary<(Type Source, Type Target), PropertyMap[]> CachedPropertyMaps =
            new();

        private sealed record PropertyMap(PropertyInfo SourceProperty, PropertyInfo TargetProperty);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="entity"></param>
        /// <param name="ensureRequired">If set to false, make sure to call <see cref="EnsureRequiredPropertiesAreSet"/> yourself.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static TDto PackToReadDto<TDto>(this IEntity entity, bool ensureRequired = true) where TDto : class, IReadDto
        {
            ArgumentNullException.ThrowIfNull(entity);

            object? dtoInstance = Activator.CreateInstance(typeof(TDto));

            if (dtoInstance is not TDto dto)
            {
                throw new InvalidOperationException($"Could not create an instance of {typeof(TDto).Name}.");
            }

            CopyMatchingProperties(entity, dto);
            if (ensureRequired)
                dto.EnsureRequiredPropertiesAreSet();

            return dto;
        }

        private static void CopyMatchingProperties(object source, object target)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(target);

            Type sourceType = source.GetType();
            Type targetType = target.GetType();

            PropertyMap[] propertyMaps = CachedPropertyMaps.GetOrAdd((sourceType, targetType),
                static key => BuildPropertyMaps(key.Source, key.Target));

            foreach (PropertyMap propertyMap in propertyMaps)
            {
                object? value = propertyMap.SourceProperty.GetValue(source);
                propertyMap.TargetProperty.SetValue(target, value);
            }
        }

        private static PropertyMap[] BuildPropertyMaps(Type sourceType, Type targetType)
        {
            PropertyInfo[] sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.CanRead).ToArray();

            Dictionary<string, PropertyInfo> targetProperties = targetType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(property => property.CanWrite)
                .ToDictionary(property => property.Name);

            List<PropertyMap> propertyMaps = [];

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                if (!targetProperties.TryGetValue(sourceProperty.Name, out PropertyInfo? targetProperty))
                {
                    continue;
                }

                if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                {
                    continue;
                }

                propertyMaps.Add(new PropertyMap(sourceProperty, targetProperty));
            }

            return propertyMaps.ToArray();
        }

        public static void EnsureRequiredPropertiesAreSet<TDto>(this TDto dto) where TDto : class, IReadDto
        {
            ArgumentNullException.ThrowIfNull(dto);

            Type type = dto.GetType();

            PropertyInfo[] requiredProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<RequiredMemberAttribute>() is not null).ToArray();

            foreach (PropertyInfo property in requiredProperties)
            {
                object? value = property.GetValue(dto);

                if (IsUnset(value, property.PropertyType))
                {
                    throw new InvalidOperationException(
                        $"Required property '{property.Name}' on '{type.Name}' was not set during mapping.");
                }
            }
        }

        private static bool IsUnset(object? value, Type propertyType)
        {
            if (value is null)
            {
                return true;
            }

            if (!propertyType.IsValueType)
                return false;

            object defaultValue = Activator.CreateInstance(propertyType)!;
            return value.Equals(defaultValue);
        }
    }
}