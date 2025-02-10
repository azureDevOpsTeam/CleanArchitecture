using DomainLayer.Common.BaseEntities;

namespace DomainLayer.Entities
{
    public class UserRole : BaseEntityModel, IAuditableEntity
    {
        public int RoleId { get; set; }

        public int UserAccountId { get; set; }

        public UserAccount UserAccount { get; set; }

        public Role Role { get; set; }
    }
}