using System.Linq.Expressions;
using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Model.Entity;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> ApplyOrderingQueryArguments<TEntity, TSearchable>(
            this IQueryable<TEntity> query,
            IComplexSearchable<TSearchable> complex)
            where TEntity : class, IEntity, ISensor
            where TSearchable : class, ISearchable, new()
        {
            IOrderedQueryable<TEntity>? orderedQuery = ApplyOrdering(
                query,
                complex.OrderByObservedAt,
                x => x.ObservedAt);

            orderedQuery = ApplyThenOrdering(
                query,
                orderedQuery,
                complex.OrderByPulledAt,
                x => x.PulledAt);

            return orderedQuery ?? query;
        }

        private static IOrderedQueryable<TEntity>? ApplyOrdering<TEntity>(
            IQueryable<TEntity> query,
            OrderDirection? direction,
            Expression<Func<TEntity, DateTime>> selector)
            where TEntity : class, IEntity, ISensor
        {
            if (!direction.HasValue)
            {
                return null;
            }

            return direction.Value switch
            {
                OrderDirection.ASCENDING => query.OrderBy(selector),
                OrderDirection.DESCENDING => query.OrderByDescending(selector),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        private static IOrderedQueryable<TEntity>? ApplyThenOrdering<TEntity>(
            IQueryable<TEntity> query,
            IOrderedQueryable<TEntity>? orderedQuery,
            OrderDirection? direction,
            Expression<Func<TEntity, DateTime>> selector)
            where TEntity : class, IEntity, ISensor
        {
            if (!direction.HasValue)
            {
                return orderedQuery;
            }

            if (orderedQuery is null)
            {
                return direction.Value switch
                {
                    OrderDirection.ASCENDING => query.OrderBy(selector),
                    OrderDirection.DESCENDING => query.OrderByDescending(selector),
                    _ => throw new ArgumentOutOfRangeException(nameof(direction))
                };
            }

            return direction.Value switch
            {
                OrderDirection.ASCENDING => orderedQuery.ThenBy(selector),
                OrderDirection.DESCENDING => orderedQuery.ThenByDescending(selector),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        public static IQueryable<TEntity> ApplyLastXDaysFilter<TEntity>(
            this IQueryable<TEntity> query, int? lastXDays, Expression<Func<TEntity, DateTime>> selector)
            where TEntity : class, IEntity
        {
            if (!lastXDays.HasValue || lastXDays.Value <= 0)
            {
                return query;
            }

            var cutoff = DateTime.UtcNow.AddDays(-lastXDays.Value);

            return query.Where(Expression.Lambda<Func<TEntity, bool>>(
                Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(cutoff)), selector.Parameters));
        }
    }
}