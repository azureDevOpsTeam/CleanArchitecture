namespace DomainLayer
{
    public interface IEntityKey<TModel>
    {
        #region Properties

        TModel Id { get; set; }

        #endregion Properties
    }
}