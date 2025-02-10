namespace DomainLayer.Common.BaseEntities
{

    public abstract class BaseEntityModel : BaseEntityModel<int>
    {
        public override int Id { get; set; }
    }

    public abstract class BaseEntityModel<T> : BaseEntity<T>
    {
        public override T Id { get; set; }
    }
}
