#region Usings

using DomainLayer.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

#endregion

namespace DomainLayer
{
    public static class AuditableShadowProperties
    {
        #region Fields

        public static readonly string CreatedByUserId = nameof(CreatedByUserId);
        public static readonly string ModifiedByUserId = nameof(ModifiedByUserId);
        public static readonly string ModifiedDateTime = nameof(ModifiedDateTime);
        public static readonly string CreatedByIp = nameof(CreatedByIp);
        public static readonly string RowVersion = nameof(RowVersion);
        //public static readonly string ModifiedByIp = nameof(ModifiedByIp);
        //public static readonly string CreatedDateTime = nameof(CreatedDateTime);

        #endregion Fields

        #region Properties

        public static readonly Func<object, int?> EFPropertyCreatedByUserId = entity => EF.Property<int?>(entity, CreatedByUserId);
        public static readonly Func<object, string> EFPropertyCreatedByIp = entity => EF.Property<string>(entity, CreatedByIp);
        public static readonly Func<object, int?> EFPropertyModifiedByUserId = entity => EF.Property<int?>(entity, ModifiedByUserId);
        public static readonly Func<object, DateTime?> EFPropertyModifiedDateTime = entity => EF.Property<DateTime?>(entity, ModifiedDateTime);
        public static readonly Func<object, byte[]> EFPropertyRowVersion = entity => EF.Property<byte[]>(entity, RowVersion);
        //public static readonly Func<object, string> EFPropertyModifiedByIp = entity => EF.Property<string>(entity, ModifiedByIp);
        //public static readonly Func<object, DateTime> EFPropertyCreatedDateTime = entity => EF.Property<DateTime>(entity, CreatedDateTime);

        #endregion Properties

        #region Methods

        #region Set & Add

        public static void SetAuditableEntityPropertyValues(this ChangeTracker changeTracker, IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor?.HttpContext;

            var addedEntries = changeTracker.Entries<IAuditableEntity>().Where(row => row.State == EntityState.Added);
            foreach (var addedEntry in addedEntries)
            {
                addedEntry.Property(CreatedByUserId).CurrentValue = GetUserId(httpContext);
                addedEntry.Property(CreatedByIp).CurrentValue = httpContext?.Connection?.RemoteIpAddress?.ToString();
                //addedEntry.Property(CreatedDateTime).CurrentValue = DateTimeOffset.UtcNow;
            }

            var modifiedEntries = changeTracker.Entries<IAuditableEntity>().Where(row => row.State == EntityState.Modified);
            foreach (var modifiedEntry in modifiedEntries)
            {
                modifiedEntry.Property(ModifiedByUserId).CurrentValue = GetUserId(httpContext);
                modifiedEntry.Property(ModifiedDateTime).CurrentValue = DateTimeOffset.UtcNow;
                //modifiedEntry.Property(ModifiedByIp).CurrentValue = httpContext?.Connection?.RemoteIpAddress?.ToString();
            }
        }

        public static void AddAuditableShadowProperties(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(row => typeof(IAuditableEntity).IsAssignableFrom(row.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType).Property<int?>(CreatedByUserId);
                modelBuilder.Entity(entityType.ClrType).Property<string>(CreatedByIp);//.HasColumnType("char(15)");
                modelBuilder.Entity(entityType.ClrType).Property<int?>(ModifiedByUserId);
                modelBuilder.Entity(entityType.ClrType).Property<DateTimeOffset?>(ModifiedDateTime);
                modelBuilder.Entity(entityType.ClrType).Property<byte[]>(RowVersion).IsRowVersion().IsRequired().IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
                //modelBuilder.Entity(entityType.ClrType).Property<string>(ModifiedByIp);//.HasColumnType("char(15)");
                //modelBuilder.Entity(entityType.ClrType).Property<DateTimeOffset>(CreatedDateTime).IsRequired();
            }
        }

        #endregion Set & Add

        #region Get

        private static int? GetUserId(HttpContext httpContext)
        {
            int? userId = null;
            var userIdValue = httpContext?.User?.Identity?.GetUserId();
            if (!string.IsNullOrWhiteSpace(userIdValue))
                userId = userIdValue.ToInt();
            return userId;
        }

        #endregion Get

        #endregion Methods
    }
}