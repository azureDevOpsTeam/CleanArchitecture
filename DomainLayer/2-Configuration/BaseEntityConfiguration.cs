
using DomainLayer.Common.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainLayer.Configuration
{
    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntityModel
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(row => row.Id);
            builder.Property(row => row.Id)
                   .ValueGeneratedOnAdd();
            builder.HasQueryFilter(d => !d.IsDeleted);
            builder.HasQueryFilter(d => d.IsActive);
        }
    }
}
