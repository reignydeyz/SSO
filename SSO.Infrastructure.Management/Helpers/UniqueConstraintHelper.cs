using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management.Helpers
{
    public static class UniqueConstraintHelper
    {
        public static TEntity HandleUniqueConstraintViolation<TEntity>(IAppDbContext context, TEntity newRecord, DbUpdateException ex) where TEntity : class
        {
            var conflictingRecord = GetConflictingRecord(context, ex, newRecord);
            if (conflictingRecord != null)
            {
                return conflictingRecord;
            }

            throw ex;
        }

        private static TEntity GetConflictingRecord<TEntity>(IAppDbContext context, DbUpdateException ex, TEntity newRecord) where TEntity : class
        {
            var entry = ex.Entries.FirstOrDefault();
            if (entry == null)
            {
                return null;
            }

            var entityType = entry.Metadata;
            var uniqueProperties = entityType.GetIndexes()
                .Where(index => index.IsUnique)
                .SelectMany(index => index.Properties)
                .Distinct();

            var query = context.Set<TEntity>().AsQueryable();

            foreach (var property in uniqueProperties)
            {
                var value = property.PropertyInfo.GetValue(newRecord);
                query = query.Where(BuildEqualsExpression<TEntity>(property.Name, value));
            }

            return query.FirstOrDefault();
        }

        private static Expression<Func<TEntity, bool>> BuildEqualsExpression<TEntity>(string propertyName, object value)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(value);
            var equals = Expression.Equal(property, constant);
            return Expression.Lambda<Func<TEntity, bool>>(equals, parameter);
        }
    }
}
