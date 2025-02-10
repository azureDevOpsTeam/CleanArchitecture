namespace DomainLayer.Common.BaseEntities
{
    public abstract class BaseEntity<T> : IBaseEntity<T>
    {
        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        public virtual T Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}