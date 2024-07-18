using Domain.Entities;

namespace Repository.Abstract.MBAbstract
{
    /// <summary>
    /// Defines the interface for the MbaOptions repository.
    /// </summary>
    public interface IMbaOptionsRepository : IRepository
    {
        /// <summary>
        /// Asynchronously creates a new <see cref="MbaOptions"/> entity with the specified country and country code.
        /// </summary>
        /// <param name="country">The country for the new <see cref="MbaOptions"/> entity.</param>
        /// <param name="countryCode">The country code for the new <see cref="MbaOptions"/> entity.</param>
        /// <returns>The newly created <see cref="MbaOptions"/> entity.</returns>
        Task<MbaOptions> CreateMbaOptionsAsync(string country, string countryCode);

        /// <summary>
        /// Asynchronously creates a new <see cref="MbaOptions"/> entity with the specified country, country code, and a list of MBAs.
        /// </summary>
        /// <param name="country">The country for the new <see cref="MbaOptions"/> entity.</param>
        /// <param name="countryCode">The country code for the new <see cref="MbaOptions"/> entity.</param>
        /// <param name="mBas">The list of MBAs for the new <see cref="MbaOptions"/> entity.</param>
        /// <returns>The newly created <see cref="MbaOptions"/> entity.</returns>
        Task<MbaOptions> CreateMbaOptionsAsync(string country, string countryCode, IList<Mba> mBas);

        /// <summary>
        /// Asynchronously adds a range of <see cref="MbaOptions"/> entities to the database.
        /// </summary>
        /// <param name="mbAOptions">The list of <see cref="MbaOptions"/> entities to add.</param>
        /// <returns>The list of added <see cref="MbaOptions"/> entities.</returns>
        Task<IList<MbaOptions>> AddRangeMbaOptionsAsync(IList<MbaOptions> mbAOptions);

        /// <summary>
        /// Asynchronously deletes a specified <see cref="MbaOptions"/> entity from the database.
        /// </summary>
        /// <param name="mbAOptions">The <see cref="MbaOptions"/> entity to delete.</param>
        Task DeleteMbaOptionsAsync(MbaOptions mbAOptions);

        /// <summary>
        /// Asynchronously deletes a range of <see cref="MbaOptions"/> entities from the database.
        /// </summary>
        /// <param name="mbAOptions">The list of <see cref="MbaOptions"/> entities to delete.</param>
        Task DeleteRangeMbaOptionsAsync(IList<MbaOptions> mbAOptions);

        /// <summary>
        /// Asynchronously retrieves a <see cref="MbaOptions"/> entity by its ID.
        /// </summary>
        /// <param name="MBAOptionsId">The ID of the <see cref="MbaOptions"/> entity to retrieve.</param>
        /// <returns>The <see cref="MbaOptions"/> entity with the specified ID.</returns>
        Task<MbaOptions> GetMbaOptionsByIdAsync(Guid MBAOptionsId);

        /// <summary>
        /// Asynchronously retrieves all <see cref="MbaOptions"/> entities from the database.
        /// </summary>
        /// <returns>A list of all <see cref="MbaOptions"/> entities.</returns>
        Task<IList<MbaOptions>> GetMbasOptionsAsync();

        /// <summary>
        /// Asynchronously updates a specified <see cref="MbaOptions"/> entity in the database.
        /// </summary>
        /// <param name="mbAOptions">The <see cref="MbaOptions"/> entity to update.</param>
        Task UpdateMbaOptionsAsync(MbaOptions mbAOptions);

        /// <summary>
        /// Asynchronously updates a range of <see cref="MbaOptions"/> entities in the database.
        /// </summary>
        /// <param name="mbAOptions">The list of <see cref="MbaOptions"/> entities to update.</param>
        Task UpdateRangeMbaOptionsAsync(IList<MbaOptions> mbAOptions);
    }
}
