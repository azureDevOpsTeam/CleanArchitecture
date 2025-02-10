using DomainLayer.Common.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InfrastructureLayer.Convention
{
    public static class SetGlobalFilter
    {
        public static void ApplyQueryFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntityModel).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    Expression filterExpression = null;

                    // Check and add IsDeleted filter
                    var isDeletedProperty = entityType.FindProperty(nameof(BaseEntityModel.IsDeleted));
                    if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
                    {
                        var isDeletedFilter = Expression.Equal(
                            Expression.Property(parameter, nameof(BaseEntityModel.IsDeleted)),
                            Expression.Constant(false)
                        );
                        filterExpression = filterExpression == null
                            ? isDeletedFilter
                            : Expression.AndAlso(filterExpression, isDeletedFilter);
                    }

                    // Check and add IsActive filter
                    var isActiveProperty = entityType.FindProperty(nameof(BaseEntityModel.IsActive));
                    if (isActiveProperty != null && isActiveProperty.ClrType == typeof(bool))
                    {
                        var isActiveFilter = Expression.Equal(
                            Expression.Property(parameter, nameof(BaseEntityModel.IsActive)),
                            Expression.Constant(true) // Assuming active records have `IsActive = true`
                        );
                        filterExpression = filterExpression == null
                            ? isActiveFilter
                            : Expression.AndAlso(filterExpression, isActiveFilter);
                    }

                    // Apply the combined filter
                    if (filterExpression != null)
                    {
                        var filter = Expression.Lambda(filterExpression, parameter);
                        entityType.SetQueryFilter(filter);
                    }
                }
            }
        }

    }
}
