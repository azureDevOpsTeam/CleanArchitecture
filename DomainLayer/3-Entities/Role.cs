using DomainLayer.Common.BaseEntities;

namespace DomainLayer.Entities
{
    public class Role : BaseEntityModel, IAuditableEntity
    {
        public string RoleName { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}