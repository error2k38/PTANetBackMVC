using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using Repository.Context;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Repository.Concrete
{
    /// <summary>
    /// Base class for database repositories.
    /// </summary>
    /// <typeparam name="T">The type of the DbContext.</typeparam>
    public abstract class DBRepositoryBase<T> : IRepository where T : DbContext
    {
        /// <summary>
        /// The connection string to the database.
        /// </summary>
        protected readonly ConnectionString ConnectionString;

        /// <summary>
        /// To detect redundant calls.
        /// </summary>
        protected bool DisposedValue = false;

        /// <summary>
        /// The database context.
        /// </summary>
        protected T _context;

        /// <summary>
        /// Initializes a new instance of the DBRepositoryBase class with a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        protected DBRepositoryBase(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
            var context = (T?)Activator.CreateInstance(typeof(T), ConnectionString.Value);
            if (context != null)
            {
                _context = (T)context;
                _context.Database.Migrate(); // Initializing or Migrating database
                _context.Dispose();
            }
        }

        /// <summary>
        /// Gets a value indicating whether a transaction is ongoing.
        /// Returns true if there is an ongoing transaction; otherwise, false.
        /// </summary>
        public bool IsInTransaction { get; protected set; }

        /// <summary>
        /// Recovery modes for the database information.
        /// </summary>
        public enum RecoveryMode
        {
            /// <summary>
            /// The operations that are executed are not logged and it is not possible to recover the database.
            /// </summary>
            SIMPLE = 0,
            /// <summary>
            /// All the operations that are executed are logged and it is possible to recover the database.
            /// </summary>
            FULL = 1
        };

        #region Delete Database

        /// <summary>
        /// Asynchronously deletes a database.
        /// </summary>
        /// <param name="dbName">The name of the database to delete.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public virtual async Task DeleteDatabaseAsync(string dbName)
        {
            var connectionString = ConnectionString.Value;
            string pattern = @"Catalog=[^;]+";
            Regex regex = new Regex(pattern);
            connectionString = regex.Replace(connectionString, $"Catalog={dbName}");

            // Check if the database exists
            await using (var tempContext = new ApplicationContext(connectionString))
            {
                if (await tempContext.Database.CanConnectAsync())
                {
                    // If the database exists, close all transactions
                    while (tempContext.Database.CurrentTransaction != null)
                    {
                        await tempContext.Database.CurrentTransaction.RollbackAsync();
                    }

                    // Delete the database
                    await tempContext.Database.EnsureDeletedAsync();
                }
            }
        }

        #endregion

        #region Transaction Management

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        public virtual void BeginTransaction()
        {
            if (!IsInTransaction)
            {
                var context = (T?)Activator.CreateInstance(typeof(T), ConnectionString.Value);
                if (context != null)
                {
                    _context = (T)context;
                    IsInTransaction = true;
                }
            }
            DisposedValue = false;
        }

        /// <summary>
        /// Commits the current changes to the database but keeps the transaction open.
        /// </summary>
        public virtual void PartialCommit()
        {
            if (!IsInTransaction)
                throw new Exception("The Repository is not in a transaction");
            try
            {
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new Exception(
                    "An error occurred during the Commit of the transaction.",
                    exception);
            }
        }

        /// <summary>
        /// Asynchronously commits the current changes to the database but keeps the transaction open.
        /// </summary>
        public virtual async Task PartialCommitAsync()
        {
            if (!IsInTransaction)
                throw new Exception("The Repository is not in a transaction");
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(
                    "An error occurred during the Commit of the transaction.",
                    exception);
            }
        }

        /// <summary>
        /// Commits the transaction, finalizing the changes to the database.
        /// </summary>
        public virtual void CommitTransaction()
        {
            if (!IsInTransaction)
                throw new Exception("The Repository is not in a transaction");
            try
            {
                _context.SaveChanges();
                _context.Dispose();
                IsInTransaction = false;
            }
            catch (Exception exception)
            {
                _context.Dispose();
                throw new Exception(
                    "An error occurred during the Commit of the transaction.",
                    exception);
            }
        }

        /// <summary>
        /// Asynchronously commits the transaction, finalizing the changes to the database.
        /// </summary>
        public virtual async Task CommitTransactionAsync()
        {
            if (!IsInTransaction)
                throw new Exception("The Repository is not in a transaction");
            try
            {
                await _context.SaveChangesAsync();
                await _context.DisposeAsync();
                IsInTransaction = false;
            }
            catch (Exception exception)
            {
                await _context.DisposeAsync();
                throw new Exception(
                    "An error occurred during the Commit of the transaction.",
                    exception);
            }
        }

        /// <summary>
        /// Rolls back the transaction, undoing any changes if it has not been committed.
        /// </summary>
        public virtual void RollbackTransaction()
        {
            if (!IsInTransaction)
                throw new Exception("The Repository is not in a transaction");
            IsInTransaction = false;
            _context.Dispose();
        }

        /// <summary>
        /// Asynchronously rolls back the transaction, undoing any changes if it has not been committed.
        /// </summary>
        public virtual async Task RollbackTransactionAsync()
        {
            if (!IsInTransaction)
                throw new Exception("The Repository is not in a transaction");
            IsInTransaction = false;
            await _context.DisposeAsync();
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A boolean value that indicates whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
        private void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects).
                    _context.Dispose();
                }
                DisposedValue = true;
            }
        }

        /// <summary>
        /// This code added to correctly implement the disposable pattern.
        /// </summary>
        public virtual void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Asynchronously retrieves a basic set of entities from the database considering criteria for filtering, ordering, and including properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="filter">Criteria for filtering.</param>
        /// <param name="orderBy">Criteria for ordering.</param>
        /// <param name="includeProperties">Properties to include.</param>
        /// <returns>A list of entities.</returns>
        public async Task<List<TEntity>> GetBasicAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "") where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            foreach (string includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            return orderBy != null
                ? await orderBy(query).ToListAsync()
                : await query.ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a set of entities from the database considering criteria for filtering, ordering, and including properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="tokenSource">A CancellationTokenSource for the cancellation of the operation.</param>
        /// <param name="filter">Criteria for filtering.</param>
        /// <param name="orderBy">Criteria for ordering.</param>
        /// <param name="includeProperties">Properties to include.</param>
        /// <returns>A list of entities.</returns>
        public async Task<List<TEntity>> GetAsync<TEntity>(CancellationTokenSource tokenSource = null, Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "") where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            foreach (string includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
            if (tokenSource == null)
                return orderBy != null
                    ? await orderBy(query).ToListAsync()
                    : await query.ToListAsync();
            return orderBy != null
                ? await orderBy(query).ToListAsync(tokenSource.Token)
                : await query.ToListAsync(tokenSource.Token);
        }

        /// <summary>
        /// Asynchronously retrieves a set of entities from the database considering criteria for filtering, ordering, and selecting properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the resulting entity.</typeparam>
        /// <param name="selector">Property selector.</param>
        /// <param name="filter">Criteria for filtering.</param>
        /// <param name="ordered">Indicator of ordering. True to get the collection ordered; otherwise false.</param>
        /// <param name="tokenSource">A CancellationTokenSource for the cancellation of the operation.</param>
        /// <returns>A list of resulting entities.</returns>
        public async Task<List<TResult>> GetAsync<TEntity, TResult>(Func<TEntity, TResult> selector,
           Expression<Func<TEntity, bool>> filter = null, bool ordered = false, CancellationTokenSource tokenSource = null) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            IQueryable<TResult> result = query.Select(selector).AsQueryable();

            return ordered
                ? await result.ToListAsync(tokenSource.Token)
                : await result.OrderBy(x => x).ToListAsync(tokenSource.Token);
        }

        /// <summary>
        /// Asynchronously retrieves an entity from the database by its ID.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID.</returns>
        public async Task<TEntity> GetByIDAsync<TEntity>(object id) where TEntity : class
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// Asynchronously adds an entity to the database.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to add.</param>
        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        /// <summary>
        /// Asynchronously adds a range of entities to the database.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities to add.</param>
        public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        /// <summary>
        /// Asynchronously deletes an entity from the database.
        /// Note that data persistence should be considered if the operation is successful.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityToDelete">The entity to delete.</param>
        public async Task DeleteAsync<TEntity>(TEntity entityToDelete) where TEntity : class
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
                _context.Set<TEntity>().Attach(entityToDelete);
            _context.Set<TEntity>().Remove(entityToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a range of entities from the database.
        /// Note that data persistence should be considered if the operation is successful.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entitiesToDelete">The entities to delete.</param>
        public async Task DeleteRangeAsync<TEntity>(IEnumerable<TEntity> entitiesToDelete) where TEntity : class
        {
            foreach (var entityToAttach in entitiesToDelete.Where(e => _context.Entry(e).State == EntityState.Detached))
                _context.Set<TEntity>().Attach(entityToAttach);
            _context.Set<TEntity>().RemoveRange(entitiesToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an entity in the database.
        /// Note that data persistence should be considered if the operation is successful.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityToUpdate">The entity to update.</param>
        public async Task UpdateAsync<TEntity>(TEntity entityToUpdate) where TEntity : class
        {
            _context.Set<TEntity>().Update(entityToUpdate);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates a range of entities in the database.
        /// Note that data persistence should be considered if the operation is successful.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entitiesToUpdate">The entities to update.</param>
        public async Task UpdateAsync<TEntity>(IEnumerable<TEntity> entitiesToUpdate) where TEntity : class
        {
            _context.Set<TEntity>().UpdateRange(entitiesToUpdate);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Attaches an entity to the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to attach.</param>
        public void AttachEntity<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
        }
    }
}
