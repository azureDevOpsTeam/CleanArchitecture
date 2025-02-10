namespace ApplicationLayer
{
    public interface IUnitOfWork
    {
        #region SaveChanges

        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());

        #endregion SaveChanges

        #region Transaction

        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();

        #endregion Transaction
    }
}