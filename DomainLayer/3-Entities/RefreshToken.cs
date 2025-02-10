using DomainLayer.Common.BaseEntities;

namespace DomainLayer.Entities
{
    public class RefreshToken : BaseEntityModel, IAuditableEntity
    {
        public int UserAccountId { get; set; }

        public string UserFullName { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime ExpiryDate { get; set; }

        #region Navigations

        public UserAccount UserAccount { get; set; }

        #endregion Navigations
    }
}