using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Context;

namespace Repository.Concrete
{
    /// <summary>
    /// Represents the ApplicationDBRepository class which is a specific implementation of the DBRepositoryBase for the ApplicationContext.
    /// This class is sealed, meaning it cannot be inherited.
    /// </summary>
    public sealed partial class ApplicationDBRepository : DBRepositoryBase<ApplicationContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDBRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string to the database.
        /// </param>
        public ApplicationDBRepository(ConnectionString connectionString) : base(connectionString)
        {
            // Create a new ApplicationContext with the provided connection string.
            _context = new ApplicationContext(ConnectionString.Value);

            // Check if the database exists.
            if (!_context.Database.GetService<IRelationalDatabaseCreator>().Exists())
            {
                // If the database does not exist, begin a new transaction.
                _context.Database.BeginTransaction();

                // Save changes to the database.
                _context.SaveChanges();

                // Commit the transaction to create the database.
                _context.Database.CommitTransaction();
            }

            // Dispose the context to free up resources.
            _context.Dispose();
        }
    }

}
