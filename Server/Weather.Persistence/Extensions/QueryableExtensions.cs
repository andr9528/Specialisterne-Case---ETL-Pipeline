using System.Linq.Expressions;
using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Model.Entity;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> ApplyOrderingQueryArguments<TEntity, TSearchable>(
            this IQueryable<TEntity> query, IComplexSearchable<TSearchable> complex)
            where TEntity : class, IEntity, ISensor where TSearchable : class, ISearchable, new()
        {
            var orderedQuery = ApplyOrdering(query, complex.OrderByObservedAt, x => x.ObservedAt);

            orderedQuery = ApplyThenOrdering(query, orderedQuery, complex.OrderByPulledAt, x => x.PulledAt);

            return orderedQuery ?? query;
        }

        private static IOrderedQueryable<TEntity>? ApplyOrdering<TEntity>(
            IQueryable<TEntity> query, OrderDirection? direction, Expression<Func<TEntity, DateTime>> selector)
            where TEntity : class, IEntity, ISensor
        {
            if (!direction.HasValue)
                return null;

            return direction.Value switch
            {
                OrderDirection.ASCENDING => query.OrderBy(selector),
                OrderDirection.DESCENDING => query.OrderByDescending(selector),
                var _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };
        }

        private static IOrderedQueryable<TEntity>? ApplyThenOrdering<TEntity>(
            IQueryable<TEntity> query, IOrderedQueryable<TEntity>? orderedQuery, OrderDirection? direction,
            Expression<Func<TEntity, DateTime>> selector) where TEntity : class, IEntity, ISensor
        {
            if (!direction.HasValue)
                return orderedQuery;

            if (orderedQuery is null)
                return direction.Value switch
                {
                    OrderDirection.ASCENDING => query.OrderBy(selector),
                    OrderDirection.DESCENDING => query.OrderByDescending(selector),
                    var _ => throw new ArgumentOutOfRangeException(nameof(direction)),
                };

            return direction.Value switch
            {
                OrderDirection.ASCENDING => orderedQuery.ThenBy(selector),
                OrderDirection.DESCENDING => orderedQuery.ThenByDescending(selector),
                var _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };
        }

        public static IQueryable<TEntity> ApplyLastXDaysFilter<TEntity>(
            this IQueryable<TEntity> query, int? lastXDays, Expression<Func<TEntity, DateTime>> selector)
            where TEntity : class, IEntity
        {
            if (!lastXDays.HasValue || lastXDays.Value <= 0)
                return query;

            DateTime cutoff = DateTime.UtcNow.AddDays(-lastXDays.Value);
            BinaryExpression body = Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(cutoff));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, selector.Parameters);

            return query.Where(lambda);
        }

        public static IQueryable<TEntity> ApplyAfterDateTime<TEntity>(
            this IQueryable<TEntity> query, DateTime? after, Expression<Func<TEntity, DateTime>> selector)
            where TEntity : class, IEntity
        {
            if (!after.HasValue)
                return query;

            BinaryExpression body = Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(after.Value));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, selector.Parameters);

            return query.Where(lambda);
        }

        public static IQueryable<TEntity> ApplyBeforeDateTime<TEntity>(
            this IQueryable<TEntity> query, DateTime? before, Expression<Func<TEntity, DateTime>> selector)
            where TEntity : class, IEntity
        {
            if (!before.HasValue)
                return query;

            BinaryExpression body = Expression.LessThanOrEqual(selector.Body, Expression.Constant(before.Value));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, selector.Parameters);

            return query.Where(lambda);
        }

        public static IQueryable<TEntity> ApplyAboveValue<TEntity, TValue>(
            this IQueryable<TEntity> query, TValue? above, Expression<Func<TEntity, TValue>> selector)
            where TEntity : class, IEntity where TValue : struct, IComparable<TValue>
        {
            if (!above.HasValue)
                return query;

            BinaryExpression body = Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(above.Value));

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, selector.Parameters);

            return query.Where(lambda);
        }

        public static IQueryable<TEntity> ApplyBelowValue<TEntity, TValue>(
            this IQueryable<TEntity> query, TValue? below, Expression<Func<TEntity, TValue>> selector)
            where TEntity : class, IEntity where TValue : struct, IComparable<TValue>
        {
            if (!below.HasValue)
                return query;

            BinaryExpression body = Expression.LessThanOrEqual(selector.Body, Expression.Constant(below.Value));

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, selector.Parameters);

            return query.Where(lambda);
        }
    }
}