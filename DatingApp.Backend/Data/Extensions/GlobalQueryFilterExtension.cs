using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DatingApp.Backend.Data.Extensions
{
    public static class GlobalQueryFilterExtension
    {
        public static void ApplyQueryFilter<TInterface>(
    this ModelBuilder modelBuilder,
    Expression<Func<TInterface, bool>> filter)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Only apply to entities that directly implement the interface
                if (!typeof(TInterface).IsAssignableFrom(entityType.ClrType))
                    continue;

                // Apply the filter to the entity
                ApplyFilterToEntity(modelBuilder, entityType, filter);
            }
        }

        private static void ApplyFilterToEntity<TInterface>(
            ModelBuilder modelBuilder,
            IMutableEntityType entityType,
            Expression<Func<TInterface, bool>> filter)
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var convertedParam = Expression.Convert(parameter, typeof(TInterface));

            var body = new ReplacingExpressionVisitor(
                filter.Parameters[0],
                convertedParam)
                .Visit(filter.Body);

            var lambda = Expression.Lambda(body, parameter);
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }

        private class ReplacingExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplacingExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }
    }
}
