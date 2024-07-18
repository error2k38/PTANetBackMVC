namespace Repository.Context
{
    /// <summary>
    /// Models a database connection string.
    /// </summary>
    public abstract class ConnectionString
    {
        /// <summary>
        /// Initializes a <see cref="ConnectionString"/> object.
        /// </summary>
        /// <param name="value">
        /// Database connection string.
        /// </param>
        public ConnectionString(string value) => Value = value;

        /// <summary>
        /// Connection string.
        /// </summary>
        public string Value { get; }
    }

    /// <summary>
    /// Models the connection string for the database.
    /// </summary>
    public class ConfigurationDBConnectionString : ConnectionString
    {
        /// <summary>
        /// Initializes a <see cref="ConfigurationDBConnectionString"/> object.
        /// </summary>
        /// <param name="value">
        /// Connection string to the database.
        /// </param>
        public ConfigurationDBConnectionString(string value) : base(value)
        {
        }
    }

}
