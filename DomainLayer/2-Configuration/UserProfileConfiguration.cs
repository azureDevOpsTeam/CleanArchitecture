using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainLayer.Configuration
{
    public class UserProfileConfiguration : BaseEntityConfiguration<UserProfile>
    {
        public override void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            base.Configure(builder);

            builder.Property(current => current.FirstName).HasMaxLength(100);
            builder.Property(current => current.LastName).HasMaxLength(100);
            builder.Property(current => current.Address).HasMaxLength(400);
            builder.HasQueryFilter(current => !current.IsDeleted);

            builder.HasOne(current => current.UserAccount).WithMany(current => current.UserProfiles).HasForeignKey(current => current.UserAccountId);
        }
    }
}