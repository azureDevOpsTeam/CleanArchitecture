namespace DomainLayer
{
    public interface ISoftDeleteEntity
    {
        #region Properties

        bool IsDeleted { get; set; }

        #endregion Properties
    }
}