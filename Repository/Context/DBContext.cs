using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context.ConnectionResources;
using Repository.FluentConfigurations.MBAFluent;

namespace Repository.Context
{
    /// <summary>
    /// Defines the database structure for the factory.
    /// </summary>
    public class ApplicationContext : DbContext
    {
        #region Tables

        /// <summary>
        /// DbSet representing the MBAs table in the database.
        /// </summary>
        public DbSet<Mba> MBAs { get; set; }

        /// <summary>
        /// DbSet representing the MBAOptions table in the database.
        /// </summary>
        public DbSet<MbaOptions> MBAOptions { get; set; }

        #endregion

        /// <summary>
        /// Default constructor required by EntityFrameworkCore for migrations.
        /// </summary>
        public ApplicationContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ApplicationContext class with a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        public ApplicationContext(string connectionString)
            : base(GetOptions(connectionString))
        {
        }

        /// <summary>
        /// Initializes a new instance of the ApplicationContext class with context options.
        /// </summary>
        /// <param name="options">The options for the context.</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the context using the provided options.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                base.OnConfiguring(optionsBuilder);
                var connectionString = Environment.GetEnvironmentVariable(DeploymentResources.DockerConnectionStrings.GetDescription(), EnvironmentVariableTarget.Machine);
                if (connectionString == null)
                    throw new ArgumentNullException("The connection string cannot be null");

                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }
            catch (Exception)
            {
                // Handle error here.. means DLL has no satellite configuration file.
                throw;
            }
        }

        /// <summary>
        /// Further configures the model that was discovered by convention from the entity types exposed in DbSet properties on your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Abstract classes mapping

            // Add configurations for abstract classes here.

            #endregion

            #region Configurations

            // Apply configurations for each entity.
            modelBuilder.ApplyConfiguration(new MBAFluentConfiguration());
            modelBuilder.ApplyConfiguration(new MBAOptionsFluentConfiguration());

            #endregion
        }

        #region Helpers

        /// <summary>
        /// Gets the options for the context using the provided connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <returns>The options for the context.</returns>
        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        #endregion
    }

}
