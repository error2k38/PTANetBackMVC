namespace Repository.Abstract
{
    /// <summary>
    /// Defines the interface for a repository. This interface includes methods for transaction management and database operations.
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether a transaction is ongoing.
        /// Returns true if there is an ongoing transaction; otherwise, false.
        /// </summary>
        bool IsInTransaction { get; }

        /// <summary>
        /// Asynchronously deletes a database.
        /// </summary>
        /// <param name="dbName">The name of the database to delete.</param>
        Task DeleteDatabaseAsync(string dbName);

        /// <summary>
        /// Begins a transaction.
        /// </summary>        
        void BeginTransaction();

        /// <summary>
        /// Commits the current changes to the database but keeps the transaction open.
        /// </summary>
        void PartialCommit();

        /// <summary>
        /// Asynchronously commits the current changes to the database but keeps the transaction open.
        /// </summary>
        Task PartialCommitAsync();

        /// <summary>
        /// Commits the transaction, finalizing the changes to the database.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Asynchronously commits the transaction, finalizing the changes to the database.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the transaction, undoing any changes if it has not been committed.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Asynchronously rolls back the transaction, undoing any changes if it has not been committed.
        /// </summary>
        Task RollbackTransactionAsync();
    }

}
