using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainLayer.Configuration
{
    public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);
            builder.HasOne(current => current.UserAccount).WithMany(current => current.UserRoles).HasForeignKey(current => current.UserAccountId);
            builder.HasOne(current => current.Role).WithMany(current => current.UserRoles).HasForeignKey(current => current.RoleId);
        }
    }
}