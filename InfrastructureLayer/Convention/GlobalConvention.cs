#region Usings

using DomainLayer;
using Microsoft.EntityFrameworkCore;

#endregion

namespace InfrastructureLayer.Convention
{
    public static class GlobalConvention
    {
        #region Fields

        private const string Id = nameof(Id);
        private const string Enums = nameof(Enums);

        #endregion Fields

        #region Methods

        #region SetGlobal

        //[Obsolete]
        public static void SetGlobalConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                //var schema = entityType.ClrType.Namespace?.Split('.').ToList().LastOrDefault();
                var schema = "dbo";
                var properties = entityType.GetProperties().ToList();

                if (!string.IsNullOrEmpty(schema) && schema != Enums)
                    modelBuilder.Entity(entityType.Name).ToTable(entityType.ClrType.Name, schema);

                foreach (var property in properties)
                {
                    if (property.ClrType == typeof(int))
                    {
                        if (property.Name == Id)
                        {
                            modelBuilder.Entity(entityType.Name).HasKey(property.Name);
                            modelBuilder.Entity(entityType.Name).Property<int>(property.Name).HasColumnName($"{entityType.ClrType.Name}{property.Name}");
                        }

                        if (typeof(IEntityIdentity<>).IsAssignableFrom(entityType.ClrType))
                            modelBuilder.Entity(entityType.Name).Property<int>(property.Name).HasComputedColumnSql(entityType.Name);
                    }

                    if (property.ClrType == typeof(Guid) && (typeof(IEntityGuid).IsAssignableFrom(entityType.ClrType)))
                        modelBuilder.Entity(entityType.Name).Property<Guid>(property.Name).HasColumnName($"{entityType.ClrType.Name}{property.Name}").IsRequired();

                    if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                        if (string.IsNullOrEmpty(property.GetColumnType()))
                            property.SetColumnType("decimal(18, 2)");
                }
            }
        }

        #endregion SetGlobal

        #endregion Methods
    }
}